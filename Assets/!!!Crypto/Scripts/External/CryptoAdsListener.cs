using System;
#if MES_HCSDK
using BoomBit.HyperCasual;
#else
#endif

namespace MES.Crypto.Ads
{
    public class CryptoAdsListener
    {
        public event Action VideoDisplayEvent;
        public event Action VideoSuccessEvent;
        public event Action InterstitialDisplayEvent;
        public event Action InterstitialCloseEvent;
        public event Action BannerDisplayEvent;
        public CryptoAdsListener()
        {
#if MES_HCSDK
            HCSDK.VideoDisplayEvent += VideoDisplay;
            HCSDK.InterstitialDisplayEvent += InterstitialDisplay;

            HCSDK.VideoSuccessEvent += VideoWatchingSuccess;
            HCSDK.InterstitialCloseEvent += InterstitialClose;

            HCSDK.BannerDisplayEvent += BannerDisplay;
#else
#endif
        }

        private void VideoDisplay()
        {
            VideoDisplayEvent?.Invoke();
        }

        private void VideoWatchingSuccess()
        {
            VideoSuccessEvent?.Invoke();
        }

        private void InterstitialDisplay()
        {
            InterstitialDisplayEvent?.Invoke();
        }
        private void InterstitialClose()
        {
            InterstitialCloseEvent?.Invoke();
        }

        private void BannerDisplay()
        {
            BannerDisplayEvent?.Invoke();
        }
        public void HideBanner()
        {
#if MES_HCSDK
            HCSDK.HideBanner();
#endif
        }
    }
}