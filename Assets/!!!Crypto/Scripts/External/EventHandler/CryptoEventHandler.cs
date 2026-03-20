using System;
using UnityEngine;

namespace MES.Crypto
{
    public class CryptoEventHandler : MonoBehaviour
    {
        public static Action OnCryptoUIClose;
        public static Action<string> OnRedeemSuccess;
    }
}