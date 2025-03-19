using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.Caching;
namespace RIAYATIEPrescriptionAPI.Services
{
    public sealed class CacheManagerService
    {
        private int _cacheDuration = string.IsNullOrEmpty(ConfigurationManager.AppSettings["CacheDuration"]) ? 30 : Convert.ToInt32(ConfigurationManager.AppSettings["CacheDuration"]);
        private static volatile CacheManagerService cacheManager = new CacheManagerService();
        private static readonly object cacheLock = new object();
        MemoryCache cache = null;
        CacheItemPolicy cacheItemPolicy = null;
        private CacheManagerService()
        {
            cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(_cacheDuration)
            };
            cache = new MemoryCache("MemCache");
        }

        public static CacheManagerService Instance
        {
            get
            {
                if (cacheManager == null)
                {
                    lock (cacheLock)
                    {
                        cacheManager = new CacheManagerService();
                    }
                }

                return cacheManager;
            }
        }

        public bool SetCache<T>(string key, T obj, int cachExpiryIn = 3600)
        {
            try
            {
                // Modify cache expiration
                if (cachExpiryIn > 0)
                {
                    cacheItemPolicy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(cachExpiryIn);
                }
                var cacheItem = new CacheItem(key, obj);
                var result = cache.Add(cacheItem, cacheItemPolicy);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Set Cache For Key {key} - {ex.Message}", ex);
            }
        }

        public T GetCache<T>(string key)
        {
            try
            {
                var result = cache.Get(key);
                return (T)result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in GetCache For Key {key} - {ex.Message}", ex);
            }
        }
        
        public void ClearCache(string key)
        {
            try
            {
                cache.Remove(key);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in remove cache For Key {key} - {ex.Message}", ex);
            }
        }
    }
}
