﻿using FindMe.Data;
using FindMe.Data.Models;
using Swapp.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FindMe.Web.App
{
    public class WebDbReader : IDisposable
    {
        public event EventHandler<ValuePairEventArgs<string, bool>> ChangeAccessToken;


        private PrjAPICmdReader _reader;

        private string _refreshTokenValue;
        private string _accessTokenValue;

        public WebDbReader(AppDbContext context)
        {
            if (context == null) throw new NullReferenceException("context");


            _reader = new PrjAPICmdReader(new DbInteractor(context));
        }


        public WebDbReader SetTokenValues(string refreshTokenValue, string accessTokenValue)
        {
            _refreshTokenValue = refreshTokenValue;
            _accessTokenValue = accessTokenValue;

            return this;
        }


        private void OnChangeAccessToken(string accessTokenValue, bool invalidPsswrdFormat)
        {
            ChangeAccessToken?.Invoke(this, new ValuePairEventArgs<string, bool>(accessTokenValue, invalidPsswrdFormat));
        }


        public void SetClientIpAddress(string value)
        {
            _reader.ClientIpAddress = value;
        }


        public async Task<object> Execute(string methodName)
        {
            return await Execute(methodName, new object[0]);
        }
        public async Task<object> Execute(string methodName, params object[] parameters)
        {
            return await Execute(new WebParameter(methodName, parameters));
        }
        public async Task<object> Execute(params WebParameter[] webParameters)
        {
            return await Execute(new WebParameterCollection(webParameters));
        }
        public async Task<object> Execute(WebParameterCollection webParameters, bool isRegenAccessTokenValue = false)
        {
            if (_reader == null) return null;


            IResponse resp = null;

            try
            {
                webParameters.AccessValue = _accessTokenValue;

                resp = await _reader.Execute(webParameters);

                if (resp.Error != null)
                {
                    if (resp.Error.GetErrorMsgID() == MessageIdentifier.ACCESS_CONNECTION_TOKEN_EXPIRED
                        && !isRegenAccessTokenValue
                        && webParameters != null)
                    {
                        if (webParameters.WebParameters == null)
                        {
                            webParameters.WebParameters = new WebParameter[0];
                        }


                        webParameters.WebParameters = (new WebParameter[] { WebParameter.Create("RegenAccessTokenValue", new object[] { _refreshTokenValue }) }).Concat(webParameters.WebParameters).ToArray();

                        return await Execute(webParameters, true);
                    }


                    throw resp.Error.ToException();
                }

                if (resp is JSonMultiResponse)
                {
                    var mResp = (JSonMultiResponse)resp;

                    if (mResp.Result != null
                        && mResp.Result.Length > 0
                        && mResp.Result[0].MethodName == "RegenAccessTokenValue")
                    {
                        var regenResp = mResp.Result[0];

                        if (regenResp.Result is AccessToken)
                        {
                            OnChangeAccessToken(((AccessToken)regenResp.Result).Value, ((AccessToken)regenResp.Result).InvalidPsswrdFormat);
                        }

                        mResp.Result = mResp.Result.Skip(1).ToArray();

                        if (mResp.Result.Length > 0
                            && mResp.Result[0].MethodName == "VerifyConnectionToken")
                        {
                            mResp.Result[0].Error = regenResp.Error;
                            mResp.Result[0].Result = regenResp.Result;
                        }

                        regenResp = null;
                    }

                    if (mResp.Result != null
                        && mResp.Result.Length == 1)
                    {
                        resp = mResp.Result[0];

                        if (resp.Error != null)
                        {
                            throw resp.Error.ToException();
                        }
                    }

                    mResp = null;
                }

                return resp.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                resp = null;
            }
        }



        public async Task<T> Execute<T>(string methodName)
        {
            return await Execute<T>(methodName, new object[0]);
        }
        public async Task<T> Execute<T>(string methodName, params object[] parameters)
        {
            return await Execute<T>(new WebParameter(methodName, parameters));
        }
        public async Task<T> Execute<T>(params WebParameter[] webParameters)
        {
            return await Execute<T>(new WebParameterCollection(webParameters));
        }
        public async Task<T> Execute<T>(WebParameterCollection webParameters)
        {
            object result;

            try
            {
                result = await Execute(webParameters);

                if (result != null
                    && typeof(T) == typeof(string))
                {
                    result = result.ToString();
                }

                return result == null ? default(T) : (T)result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                result = null;
            }
        }



        public void Dispose()
        {
            if (_reader != null)
            {
                _reader.Dispose();
                _reader = null;
            }

            _refreshTokenValue = null;
            _accessTokenValue = null;
        }
    }
}
