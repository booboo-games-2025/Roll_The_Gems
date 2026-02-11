using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    [Header("Leaderboard Config")]
    [SerializeField] GameObject leaderBoardGO;
    [SerializeField] private Transform leaderBoardListContainer;
    [SerializeField] private GameObject leaderboardItemPrefab;
    [SerializeField] private GameObject playerLeaderboardItem;
    [SerializeField] private ScrollRect leaderboardScrollRect;
    [SerializeField] private List<GameObject> leaderboardSpawnList;
    [SerializeField] Sprite[] countryIcons;
    string[] leaderboardNames;
    [SerializeField] private GameObject silverSection;
    [SerializeField] private GameObject goldSection;
    [SerializeField] UiButton jumpRankButton;
    [SerializeField] RectTransform leaderboardHeaderRt;
    private Transform contentObj;
    public bool canOverrideScore;
    public int overrideScore;
    public int customrankUp;
    public float verticalScrollRect;

    private void Start()
    {
        verticalScrollRect = leaderboardScrollRect.verticalNormalizedPosition;
    }

    private int newScore = 0;
    public void ShowLeaderBoard()
    {
        if (newScore >= EconomyManager.instance.permanentCoinCount)
        {
            leaderBoardGO.SetActive(true);
            return;
        }
        leaderboardSpawnList.ForEach(x => Destroy(x));
        leaderboardSpawnList.Clear();
        UpdateLeaderboardButtons(PlayerRank);
        Canvas.ForceUpdateCanvases();
     
        int rankUpAmount = 20;

        float multiplierFactor = Mathf.Clamp01(ExtensionMethods.Map((float)EconomyManager.instance.permanentCoinCount, 300, 1000, 0.1f, 1));
        rankUpAmount = Mathf.RoundToInt(rankUpAmount * multiplierFactor);

        newScore = (int)EconomyManager.instance.permanentCoinCount;
        int newRank = 0;
        
        int fixedScore = newScore;

        if (fixedScore > 100000000)
        {
            fixedScore = 100000000;
        }

        newRank = (int)ExtensionMethods.Map(fixedScore, 0, 100000000, 10000, 2);
        newRank = Mathf.Clamp(newRank, 2, 10000);
        if (PlayerRank < newRank) PlayerRank = 10000;
        rankUpAmount = PlayerRank - newRank;
        if (PlayerRank <= 100)
        {
            rankUpAmount = Mathf.RoundToInt(rankUpAmount * 0.1f);
        }
        if (rankUpAmount < 0)
            rankUpAmount = 0;

        Debug.Log("New Score: " + newScore + " Player Rank: " + PlayerRank + " Rankup amount: " + rankUpAmount + " New Rank: " + newRank);

        InitializeLeaderBoard(PlayerTotalScore, PlayerRank, newScore, rankUpAmount);
    }
    void UpdateLeaderboardButtons(int forPlayerrank)
    {
        jumpRankButton.gameObject.SetActive(true);
        jumpRankButton.gameObject.SetActive(forPlayerrank > 100);
    }

    int PlayerRank
    {
        get
        {
            return PlayerPrefs.GetInt("LeaderboardRank", 10000);
        }

        set
        {
            PlayerPrefs.SetInt("LeaderboardRank", value);
            PlayerPrefs.Save();
        }
    }

    int PlayerTotalScore
    {
        get
        {
            return PlayerPrefs.GetInt("PlayerTotalScore", 0);
        }

        set
        {
            PlayerPrefs.SetInt("PlayerTotalScore", value);
            PlayerPrefs.Save();
        }
    }

    public void InitializeLeaderBoard(int playerScore, int playerRank, int newScore, int rankup, float animationDelay = 1f)
    {
        contentObj = leaderBoardListContainer; // storing parent obj in temp var

        int fixedScore = newScore;

        while (fixedScore > 100000000)
            fixedScore -= 100000000;

        int playersAbovePlayer = 2;
        int playersBelowPlayer = 4;
        int maxScore = 100000000;
        int totalPlayers = 10000;

        int numberOfBarsToSpawn = rankup + playersAbovePlayer + playersBelowPlayer;
        int currentRank = playerRank - (rankup + playersAbovePlayer);

        int increments = maxScore / totalPlayers;

        leaderboardNames = new string[] {
  "BibimbapCharger461",
  "BibimbapJester",
  "BigBurger_Fury",
  "BigCandy_Invoker",
  "BigCheeseMaster",
  "BigColaInvoker",
  "BigCola_Warden",
  "BigCurry-Reaper",
  "BigDonut_Knight",
  "BigDosaProwler",
  "BigEspresso-Knight",
  "BigFalafelMaster",
  "BigFriesStorm",
  "BigFries_Rider",
  "BigGarlicQueen",
  "BigMilkshake-Slinger",
  "BigPaneer_Captain",
  "BigPaneer_Invoker",
  "BigPopcorn_Commander",
  "BigQueso-Survivor",
  "BigSchnitzel-Grinder",
  "BigSchnitzelLancer",
  "BigTofuChallenger",
  "BigTuna-Wizard",
  "Big_BurgerNinja",
  "Big_FalafelBlaster",
  "Big_GinGoblin",
  "Big_MacaronKing",
  "Big_MuffinStorm",
  "Big_TunaCrusher",
  "BobaCrusader",
  "BobaDrKnight",
  "BobaDuelist833",
  "BobaFireCommander",
  "BobaFireWarden",
  "BobaKid557",
  "BobaMaster",
  "BobaMaster781",
  "BobaMrNinja",
  "BobaNeoCaptain",
  "BobaProwler452",
  "BobaProwler705",
  "BobaSilentKid",
  "BobaSlinger",
  "BobaSurvivor886",
  "BobaWarden263",
  "BrownieBandit501",
  "BrownieKid783",
  "BrownieLilCharger",
  "BurgerFunkyHunter",
  "BurgerFunkyKid",
  "BurgerKnight805",
  "BurgerPaladin588",
  "Burger_Agent",
  "BurritoHero409",
  "BurritoMegaDuelist",
  "BurritoTank953",
  "BurritoUltraCrusher",
  "Candy-Survivor",
  "CandyBoss442",
  "CandyMissAgent",
  "CandyProtoOverlord",
  "ChaiBigLancer",
  "ChaiFireFury",
  "ChaiFunkyCommander",
  "ChaiHandler817",
  "ChaiKnight887",
  "ChaiMegaSlinger",
  "ChaiMissBuster",
  "ChaiWizard137",
  "CheeseBeast508",
  "CheeseLilJester",
  "CheeseTank530",
  "ChiliBoss417",
  "ChiliCharger998",
  "ChiliDrDealer",
  "ChiliGhost315",
  "ChiliHyperWizard",
  "ChiliJester921",
  "ChilixNinja",
  "Churro-Grinder",
  "ChurroBandit543",
  "ChurroHunter858",
  "ChurroNinja960",
  "ChurroWarden809",
  "ColaLightJester",
  "ColaSeeker316",
  "ColaSilentSlinger",
  "ColaxReaper",
  "CookieCyberStorm",
  "CookieDrBoss",
  "CookieFunkyFury",
  "CookieInvoker632",
  "CookieMissAgent",
  "CookieUltraDuelist",
  "CupcakeFunkyQueen",
  "CupcakeOverlord372",
  "CupcakeUltraCaptain",
  "CupcakeWizard329",
  "CurryCyberOverlord",
  "CurryGoblin",
  "CurryMancer807",
  "CurryProtoMenace",
  "CurrySurvivor49",
  "Curry_King",
  "CyberBoba-Invoker",
  "CyberBobaJester",
  "CyberBurger_Buster",
  "CyberCola-Master",
  "CyberCola_Invoker",
  "CyberDonut_Menace",
  "CyberEspresso-Scout",
  "CyberGarlic_Menace",
  "CyberGin-Titan",
  "CyberKetchup-Mage",
  "CyberKimchi_Challenger",
  "CyberMatchaHero",
  "CyberOnionCrusader",
  "CyberPasta_Seeker",
  "CyberPickle-Seeker",
  "CyberRamen-Buster",
  "CyberRiceKing",
  "CyberSalsaAgent",
  "CyberSalsa_Ninja",
  "CyberSchnitzelCommander",
  "CyberSoda-Titan",
  "CyberSteak-Warden",
  "CyberSushi_Commander",
  "CyberTacoTitan",
  "CyberTofu_Mage",
  "CyberTunaOverlord",
  "Cyber_CandyHero",
  "Cyber_CookieGoblin",
  "Cyber_KimchiCaptain",
  "Cyber_MatchaDealer",
  "Cyber_MilkshakeKing",
  "Cyber_MilkshakeMancer",
  "Cyber_PancakeCrusader",
  "Cyber_RamenBuster",
  "Cyber_RiceScout",
  "Cyber_SushiPaladin",
  "DarkBibimbapPaladin",
  "DarkBoba_Knight",
  "DarkBrownieScout",
  "DarkCookie_Crusader",
  "DarkDosaWizard",
  "DarkGinHunter",
  "DarkGinSeeker",
  "DarkGin_Menace",
  "DarkGravy_Prowler",
  "DarkJelly_Mancer",
  "DarkLatteGrinder",
  "DarkLatteKid",
  "DarkMatcha-Crusher",
  "DarkMochaStorm",
  "DarkMocha_Menace",
  "DarkMuffin-Duelist",
  "DarkPaneer_Hunter",
  "DarkPastaMaster",
  "DarkPasta_Paladin",
  "DarkQueso-Prowler",
  "DarkQueso_Duelist",
  "DarkRiceMenace",
  "DarkRice_Slinger",
  "DarkSamosaMaster",
  "DarkSchnitzelMage",
  "DarkSteak-Charger",
  "DarkSushi-Menace",
  "DarkTempura-Crusader",
  "DarkTempuraBoss",
  "DarkWaffle_Challenger",
  "DarkWhiskey_Queen",
  "Dark_CandyCharger",
  "Dark_ChaiRider",
  "Dark_ChiliSniper",
  "Dark_ChurroOverlord",
  "Dark_JellyBeast",
  "Dark_OnionSniper",
  "Dark_PizzaCommander",
  "DonutBoss699",
  "DonutCharger171",
  "DonutHyperChamp",
  "DonutLilBuster",
  "DonutMaster762",
  "DonutMaster783",
  "DonutMonk715",
  "DonutNinja599",
  "DonutSilentHero",
  "DosaBigQueen",
  "DosaDrCommander",
  "DosaHunter974",
  "DosaMonk622",
  "DosaMrDuelist",
  "DosaScout834",
  "DosaTitan",
  "DrBurrito-Commander",
  "DrCandy_Grinder",
  "DrCheeseReaper",
  "DrChili_Sniper",
  "DrCookie-Monk",
  "DrCurry_Crusader",
  "DrEspressoStorm",
  "DrGravy_Dealer",
  "DrJellyMage",
  "DrKimchi_Fury",
  "DrKimchi_Slinger",
  "DrMacaron-Jester",
  "DrMayo-King",
  "DrNacho-Challenger",
  "DrNacho-Commander",
  "DrNacho_Mancer",
  "DrPopcornHero",
  "DrQuesoPaladin",
  "DrRamen-Hero",
  "DrRice_Crusher",
  "DrSalsaBandit",
  "DrTaco-Captain",
  "Dr_ChurroStorm",
  "Dr_JellyBuster",
  "Dr_MacaronScout",
  "Dr_MilkshakeCommander",
  "Dr_MochaSniper",
  "Dr_NuggetTank",
  "Dr_OnionWarden",
  "Dr_SushiFury",
  "Dumpling-Prowler",
  "DumplingCyberNinja",
  "DumplingHyperLancer",
  "DumplingRider255",
  "DumplingSilentCommander",
  "Dumpling_Rider",
  "EspressoCommander596",
  "EspressoCyberGrinder",
  "EspressoKing372",
  "EspressoKing500",
  "EspressoMenace416",
  "EspressoMonk",
  "EspressoNeoJester",
  "EspressoSurvivor",
  "EspressoUltraWarden",
  "Espresso_Queen",
  "FalafelBigKing",
  "FalafelDarkCaptain",
  "FalafelHyperCharger",
  "FalafelInvoker255",
  "FalafelNinja565",
  "FalafelRogue",
  "FalafelScout328",
  "FalafelSilentChallenger",
  "FalafelWarden158",
  "FalafelWizard539",
  "Falafel_Agent",
  "FireBrownie_Tank",
  "FireBurrito-Jester",
  "FireChurro_Wizard",
  "FireCola-Menace",
  "FireCookie-Invoker",
  "FireCookieBoss",
  "FireCupcakeFury",
  "FireDosa-Agent",
  "FireGin-Wizard",
  "FireHotdogRider",
  "FireJellyGoblin",
  "FireKetchup-Overlord",
  "FireKimchi_Buster",
  "FireLatteHandler",
  "FireMacaron_Wizard",
  "FireMatchaBlaster",
  "FireMayo_Mancer",
  "FireMocha_Commander",
  "FireMocha_Sniper",
  "FireNoodleAgent",
  "FirePancake_Reaper",
  "FireSchnitzelNinja",
  "FireSoda_Prowler",
  "FireSteakTitan",
  "FireTacoMonk",
  "FireTempuraMage",
  "FireWaffleProwler",
  "FireWaffle_Tank",
  "Fire_GinGhost",
  "Fire_IceCreamKid",
  "Fire_KimchiGrinder",
  "Fire_PaneerWizard",
  "Fire_PhoDuelist",
  "Fire_RiceQueen",
  "Fire_TacoTank",
  "Fire_WaffleRider",
  "FriesBandit871",
  "FriesBeast755",
  "FriesBlaster",
  "FriesBoss",
  "FriesKnight655",
  "FriesMaster647",
  "FriesOverlord604",
  "FriesProtoChamp",
  "FunkyBobaWizard",
  "FunkyCandy_Beast",
  "FunkyCheese_Dealer",
  "FunkyCheese_Storm",
  "FunkyChiliCharger",
  "FunkyChurro_Commander",
  "FunkyDonut_Kid",
  "FunkyDumpling-Invoker",
  "FunkyFalafelBoss",
  "FunkyFalafelNinja",
  "FunkyGarlicProwler",
  "FunkyGin_Blaster",
  "FunkyGin_Handler",
  "FunkyGravy-Jester",
  "FunkyJelly_Warden",
  "FunkyMocha-Handler",
  "FunkyMocha_Scout",
  "FunkyMuffin-Buster",
  "FunkyNugget-Fury",
  "FunkyOnion-Monk",
  "FunkyPastaChamp",
  "FunkyPho_Kid",
  "FunkyPizza-Blaster",
  "FunkyPopcorn-Prowler",
  "FunkyQueso-Warden",
  "FunkyRice_Monk",
  "FunkySalsa-Survivor",
  "FunkySalsa-Warden",
  "FunkySoda-Commander",
  "FunkySteakWizard",
  "FunkySteak_Boss",
  "FunkySteak_Champ",
  "FunkySushi-Boss",
  "FunkyTofu_Hunter",
  "FunkyTunaChallenger",
  "Funky_CookieBandit",
  "Funky_DonutRider",
  "Funky_QuesoMancer",
  "Funky_SalsaMage",
  "Funky_SchnitzelNinja",
  "Garlic-Invoker",
  "GarlicAgent920",
  "GarlicCrusader495",
  "GarlicHyperMancer",
  "GarlicKnight983",
  "GarlicxWizard",
  "Gin-Crusher",
  "GinDealer",
  "GinFunkyAgent",
  "GinFunkyNinja",
  "GinKing686",
  "GinQueen293",
  "GinUltraTitan",
  "GravyAgent748",
  "GravyHyperCharger",
  "GravyTheSeeker",
  "GravyWizard",
  "HotdogCyberReaper",
  "HotdogDarkMancer",
  "HotdogDuelist893",
  "HotdogMegaWizard",
  "HotdogMissCrusader",
  "HotdogReaper204",
  "HotdogSilentCrusher",
  "HyperBobaBlaster",
  "HyperCandy-Challenger",
  "HyperCandyFury",
  "HyperCheese_Kid",
  "HyperChurro_Bandit",
  "HyperIceCream_Crusher",
  "HyperMilkshake-Wizard",
  "HyperMilkshakeReaper",
  "HyperMochaHandler",
  "HyperNacho_King",
  "HyperNugget_Ninja",
  "HyperOnionSeeker",
  "HyperPickle-Wizard",
  "HyperPizza-Scout",
  "HyperPizza_Blaster",
  "HyperRamen-Mancer",
  "HyperSushi_Ghost",
  "HyperTaco_Slinger",
  "HyperTempura_Mage",
  "HyperTofuCrusher",
  "HyperTuna_Challenger",
  "HyperWaffleBandit",
  "Hyper_GarlicGoblin",
  "Hyper_MuffinKing",
  "Hyper_NachoGoblin",
  "Hyper_OnionBoss",
  "IceCreamBuster",
  "IceCreamCommander420",
  "IceCreamDarkCaptain",
  "IceCreamHunter",
  "IceCreamJester906",
  "IceCreamKid247",
  "IceCreamLightMancer",
  "IceCreamMrMonk",
  "IceCreamReaper893",
  "Jelly-Boss",
  "JellyMaster737",
  "JellyMissBoss",
  "JellyMissWarden",
  "KetchupNinja22",
  "KimchiCaptain",
  "KimchiHyperBlaster",
  "KimchiJester518",
  "KimchiMegaReaper",
  "KimchiRider",
  "KimchixCharger",
  "LatteCrusader560",
  "LatteFunkyCommander",
  "LatteGhost916",
  "LatteHyperReaper",
  "LatteLightGoblin",
  "LatteProtoCaptain",
  "LatteReaper587",
  "LightBoba-Ghost",
  "LightBobaProwler",
  "LightBoba_Rider",
  "LightChili-Challenger",
  "LightCookie-Commander",
  "LightCookie_Boss",
  "LightCookie_Rider",
  "LightCurry-Seeker",
  "LightDumplingNinja",
  "LightGravy_Overlord",
  "LightKetchup-Ninja",
  "LightMilkshake-Menace",
  "LightMilkshake_Crusher",
  "LightPasta_Duelist",
  "LightPho-King",
  "LightPickle-Wizard",
  "LightSalsa-Boss",
  "LightSoda-Duelist",
  "LightSushi_Paladin",
  "LightWhiskey-Warden",
  "Light_CheeseTitan",
  "Light_ColaCrusader",
  "Light_KimchiSurvivor",
  "Light_OnionCaptain",
  "Light_PastaGhost",
  "Light_QuesoInvoker",
  "Light_SchnitzelLancer",
  "Light_WhiskeyProwler",
  "LilBibimbapKid",
  "LilChiliKing",
  "LilColaMaster",
  "LilColaTank",
  "LilCookie-Commander",
  "LilCupcakeAgent",
  "LilDonut_Prowler",
  "LilDosa_Slinger",
  "LilDumplingBandit",
  "LilEspresso-Knight",
  "LilFries-Duelist",
  "LilGin-Queen",
  "LilLatte_Crusher",
  "LilMatchaSurvivor",
  "LilMayo_Captain",
  "LilNugget_Slinger",
  "LilPancake-Paladin",
  "LilPancakeReaper",
  "LilPastaCrusher",
  "LilPasta_Invoker",
  "LilPickle_Goblin",
  "LilRamenProwler",
  "LilRice_Hero",
  "LilSalsa_Commander",
  "LilSteak_Knight",
  "LilTaco_Rider",
  "LilTempura_Prowler",
  "LilTofu_Dealer",
  "LilWhiskey-Scout",
  "Lil_ChaiChamp",
  "Lil_ChaiGhost",
  "Lil_DonutMaster",
  "Lil_FalafelCommander",
  "Lil_FalafelMonk",
  "Lil_GarlicSeeker",
  "Lil_LatteCharger",
  "Lil_MatchaHero",
  "Lil_PancakeRogue",
  "Lil_PastaWizard",
  "Lil_SalsaBeast",
  "Lil_SalsaDuelist",
  "Lil_SteakDealer",
  "MacaronCaptain509",
  "MacaronNeoChallenger",
  "MacaronSeeker126",
  "MacaronSniper",
  "MacaronUltraLancer",
  "MatchaBigBandit",
  "MatchaBuster",
  "MatchaJester638",
  "MatchaMissBoss",
  "MatchaSurvivor284",
  "MayoBandit734",
  "MayoDealer965",
  "MayoGhost",
  "MayoHyperHunter",
  "MayoHyperKnight",
  "MayoLilOverlord",
  "MayoScout586",
  "MegaBibimbap-Grinder",
  "MegaBurritoWizard",
  "MegaBurrito_Wizard",
  "MegaCandy-Commander",
  "MegaCandy-Knight",
  "MegaCandyScout",
  "MegaChai-Prowler",
  "MegaCheese-Tank",
  "MegaChili-Bandit",
  "MegaCookie-Mancer",
  "MegaCookieTitan",
  "MegaDumpling_Kid",
  "MegaFalafel_Rogue",
  "MegaFries-Champ",
  "MegaGarlic_Bandit",
  "MegaGarlic_Survivor",
  "MegaGravy_Sniper",
  "MegaHotdog_King",
  "MegaKimchi-Buster",
  "MegaKimchiMonk",
  "MegaMayoMancer",
  "MegaMilkshakeJester",
  "MegaMilkshake_Queen",
  "MegaMuffin_Dealer",
  "MegaNoodleDuelist",
  "MegaPancake-Slinger",
  "MegaPopcorn-King",
  "MegaQueso-Survivor",
  "MegaQuesoJester",
  "MegaSamosa_Wizard",
  "MegaSteak_Monk",
  "MegaTacoBoss",
  "MegaTacoKid",
  "MegaTofu-Charger",
  "MegaWaffle_Dealer",
  "MegaWhiskey-Seeker",
  "Mega_BurgerRider",
  "Mega_BurgerTank",
  "Mega_ColaNinja",
  "Mega_EspressoDuelist",
  "Mega_IceCreamCaptain",
  "Mega_SalsaStorm",
  "Mega_TacoRider",
  "Milkshake-Hunter",
  "MilkshakeBoss",
  "MilkshakeFunkyInvoker",
  "MilkshakeFunkyMonk",
  "MilkshakeJester",
  "MilkshakeUltraGrinder",
  "MilkshakeWizard424",
  "MissBibimbapMancer",
  "MissBibimbap_Duelist",
  "MissBobaScout",
  "MissBoba_Goblin",
  "MissChai_Scout",
  "MissChurro_Survivor",
  "MissCola_Crusader",
  "MissCookie-Captain",
  "MissCookie-Queen",
  "MissCurryRider",
  "MissCurry_Mage",
  "MissDonutReaper",
  "MissDosa-Duelist",
  "MissGin_Kid",
  "MissHotdog_Challenger",
  "MissJelly_Tank",
  "MissKetchupLancer",
  "MissKimchi_Crusher",
  "MissMacaron-Crusher",
  "MissMacaronKing",
  "MissMocha-Bandit",
  "MissNachoHunter",
  "MissNacho_Invoker",
  "MissPancake_Sniper",
  "MissPasta-Hero",
  "MissPasta-Ninja",
  "MissPasta_Agent",
  "MissPopcorn_Commander",
  "MissSamosa_Captain",
  "MissSoda-Monk",
  "MissWhiskey-King",
  "Miss_ChaiTank",
  "Miss_DumplingRogue",
  "Miss_GinCharger",
  "Miss_GravyGhost",
  "Miss_KimchiRider",
  "Miss_MochaAgent",
  "Miss_PancakePaladin",
  "Miss_PaneerCrusader",
  "Miss_PastaSniper",
  "Miss_RiceOverlord",
  "Mocha-Ghost",
  "Mocha-Storm",
  "MochaBeast366",
  "MochaChallenger12",
  "MochaDarkSurvivor",
  "MochaLightSlinger",
  "MochaNeoSeeker",
  "MochaRider637",
  "MochaSilentPaladin",
  "MochaSlinger99",
  "MochaSniper162",
  "MrBobaHandler",
  "MrBoba_Monk",
  "MrCookie-Goblin",
  "MrDosa-Scout",
  "MrDumplingSlinger",
  "MrFries_Rider",
  "MrGarlic_Grinder",
  "MrGin-Master",
  "MrGin_Scout",
  "MrGravyMage",
  "MrKetchup-Invoker",
  "MrKetchup-Mage",
  "MrMocha_Dealer",
  "MrMuffinGrinder",
  "MrMuffinHunter",
  "MrMuffin_Rider",
  "MrNacho_Monk",
  "MrNugget_Blaster",
  "MrPancakeTank",
  "MrPasta-Boss",
  "MrPizzaRogue",
  "MrQuesoSurvivor",
  "MrSamosa-Boss",
  "MrSamosa_Dealer",
  "MrSteak-Slinger",
  "MrTofu_Seeker",
  "MrWaffle_Seeker",
  "MrWhiskey-Prowler",
  "Mr_CandyHandler",
  "Mr_CandyProwler",
  "Mr_ChaiGhost",
  "Mr_DumplingInvoker",
  "Mr_IceCreamHunter",
  "Mr_JellyMaster",
  "Mr_TofuMenace",
  "Mr_TunaMancer",
  "MuffinKing763",
  "MuffinMage757",
  "MuffinNeoSniper",
  "MuffinSniper",
  "NachoBandit802",
  "NachoCaptain826",
  "NachoCrusader10",
  "NachoDarkMage",
  "NachoMissTitan",
  "NachoNinja880",
  "NachoSilentCharger",
  "NachoUltraBlaster",
  "NeoBobaSniper",
  "NeoChai_Ninja",
  "NeoCheese-Blaster",
  "NeoCheese_Paladin",
  "NeoCookieCrusader",
  "NeoDonutNinja",
  "NeoEspresso-Duelist",
  "NeoGarlic_Grinder",
  "NeoGarlic_Tank",
  "NeoGravy-Jester",
  "NeoJelly-Overlord",
  "NeoMacaron-Agent",
  "NeoOnion-Buster",
  "NeoPasta-Warden",
  "NeoPickle-Crusher",
  "NeoPizzaReaper",
  "NeoRamen-Knight",
  "NeoSamosa_Kid",
  "NeoSchnitzel_Scout",
  "NeoSteakCrusher",
  "NeoTaco-Reaper",
  "NeoTofuBuster",
  "NeoTofu_Invoker",
  "Neo_BurgerBoss",
  "Neo_ColaNinja",
  "Neo_CookieInvoker",
  "Neo_GinCommander",
  "Neo_GravyCharger",
  "Neo_IceCreamMenace",
  "Neo_NoodleCrusher",
  "Neo_PhoMaster",
  "Neo_RamenFury",
  "Neo_SushiLancer",
  "Neo_TempuraBandit",
  "Neo_TempuraFury",
  "Neo_TunaBoss",
  "NoodleFireWizard",
  "NoodleFunkyBoss",
  "NoodleFunkySeeker",
  "NoodleGoblin52",
  "NoodleLilCaptain",
  "NoodleMenace297",
  "NoodleMonk255",
  "NoodleNeoQueen",
  "NoodleSurvivor406",
  "NoodleTheKnight",
  "NuggetBoss577",
  "NuggetCyberSniper",
  "NuggetDarkTitan",
  "NuggetMegaCharger",
  "NuggetMrOverlord",
  "NuggetTitan70",
  "Onion-Sniper",
  "OnionDarkChallenger",
  "OnionDrChallenger",
  "OnionFireWarden",
  "OnionJester673",
  "OnionKing",
  "OnionRogue847",
  "OnionSilentRider",
  "PancakeFireCrusader",
  "PancakeFunkyMenace",
  "PancakeGoblin390",
  "PancakeHyperKing",
  "PancakeSilentPaladin",
  "PancakeSurvivor273",
  "PancakeSurvivor397",
  "Paneer-Boss",
  "PaneerBandit",
  "PaneerChamp",
  "PaneerChamp166",
  "PaneerDarkMancer",
  "PaneerFunkyCharger",
  "PaneerGrinder11",
  "PaneerHero",
  "PaneerSlinger718",
  "PaneerSlinger730",
  "PaneerUltraWarden",
  "PaneerWizard829",
  "PastaFunkyChamp",
  "PastaHandler80",
  "PastaHero",
  "PastaLilKnight",
  "Pho-Bandit",
  "PhoBuster688",
  "PhoKid497",
  "PhoLancer849",
  "PhoMrSeeker",
  "PhoSniper755",
  "PhoWizard958",
  "PickleAgent",
  "PickleCrusader627",
  "PickleCyberBlaster",
  "PickleDarkAgent",
  "PickleProwler",
  "PickleWizard316",
  "PizzaChamp",
  "PizzaCyberHero",
  "PizzaFunkyTitan",
  "PizzaKing791",
  "PizzaSlinger812",
  "PizzaWizard767",
  "PopcornCyberMenace",
  "PopcornDuelist",
  "PopcornFireCommander",
  "PopcornFunkyBoss",
  "PopcornHyperSurvivor",
  "PopcornMaster447",
  "PopcornMissAgent",
  "PopcornNeoGhost",
  "ProtoBoba-Reaper",
  "ProtoBurrito-Commander",
  "ProtoChaiBandit",
  "ProtoCookie-Commander",
  "ProtoDonut_Knight",
  "ProtoDosaInvoker",
  "ProtoDumplingBandit",
  "ProtoGin_Titan",
  "ProtoHotdog-Seeker",
  "ProtoMocha-Reaper",
  "ProtoMochaGrinder",
  "ProtoNugget-Prowler",
  "ProtoOnion_Crusher",
  "ProtoPasta_Commander",
  "ProtoPasta_Monk",
  "ProtoSamosa-Seeker",
  "ProtoSchnitzelSeeker",
  "ProtoTacoGoblin",
  "Proto_CurryProwler",
  "Proto_GinSniper",
  "Proto_JellyFury",
  "Proto_KimchiMage",
  "Proto_MuffinSeeker",
  "Proto_RiceGrinder",
  "Proto_RiceHandler",
  "Proto_WaffleSurvivor",
  "QuesoBigDealer",
  "QuesoBoss",
  "QuesoGoblin58",
  "QuesoKid625",
  "QuesoSniper170",
  "RamenBigHandler",
  "RamenCyberFury",
  "RamenMaster750",
  "RamenProtoQueen",
  "RiceBigSeeker",
  "RiceNinja536",
  "RiceNinja819",
  "RicePaladin764",
  "RiceProwler390",
  "Salsa-Scout",
  "SalsaJester633",
  "SalsaLightKnight",
  "SalsaLilFury",
  "SalsaSlinger554",
  "SamosaBigKnight",
  "SamosaCrusher869",
  "SamosaDarkRogue",
  "SamosaJester910",
  "SamosaKing851",
  "SamosaProtoNinja",
  "SchnitzelMissPaladin",
  "SchnitzelPaladin966",
  "SchnitzelSniper813",
  "SchnitzelStorm793",
  "SilentBibimbap-Rider",
  "SilentBurritoAgent",
  "SilentCheese_Scout",
  "SilentChiliCommander",
  "SilentChurro-Crusher",
  "SilentCola_Menace",
  "SilentCupcake-Charger",
  "SilentDumplingWarden",
  "SilentFalafel-Tank",
  "SilentFries-Mage",
  "SilentGin-Bandit",
  "SilentGravy-Goblin",
  "SilentKetchupWizard",
  "SilentLatte_Ghost",
  "SilentMacaron_Wizard",
  "SilentMilkshake-Ninja",
  "SilentMocha-Handler",
  "SilentPaneer_Sniper",
  "SilentPastaRider",
  "SilentQuesoSeeker",
  "SilentSalsa-Handler",
  "SilentSchnitzelRider",
  "SilentSodaBoss",
  "SilentTempura_Grinder",
  "SilentTunaMenace",
  "SilentWaffleInvoker",
  "Silent_BibimbapTank",
  "Silent_EspressoRogue",
  "Silent_MacaronHero",
  "Silent_PizzaBuster",
  "SodaDarkMancer",
  "SodaInvoker216",
  "SodaMrSniper",
  "SodaQueen",
  "Steak-Fury",
  "SteakCrusher",
  "SteakFunkySurvivor",
  "SteakMage101",
  "SteakMenace180",
  "SteakNeoMonk",
  "Sushi-Goblin",
  "SushiMenace608",
  "SushiMonk",
  "SushiNeoAgent",
  "SushiSurvivor181",
  "SushiTitan622",
  "TacoDarkCommander",
  "TacoDarkOverlord",
  "TacoGoblin67",
  "TacoInvoker",
  "TacoInvoker477",
  "TacoMegaInvoker",
  "TacoProwler409",
  "TacoSniper691",
  "TempuraBandit99",
  "TempuraChallenger319",
  "TempuraKid",
  "TempuraKing954",
  "TempuraMegaBlaster",
  "TempuraNeoGhost",
  "TheBibimbap-Slinger",
  "TheBibimbap_Jester",
  "TheBurger-Duelist",
  "TheBurger_Scout",
  "TheChaiTitan",
  "TheChurro-Master",
  "TheCookie_Captain",
  "TheCurrySniper",
  "TheDosa-Jester",
  "TheIceCream_Mage",
  "TheMayoQueen",
  "TheMilkshake-Lancer",
  "TheMocha_Rider",
  "TheNachoStorm",
  "TheNacho_Monk",
  "ThePancake-Rogue",
  "ThePancakeSlinger",
  "TheQueso_Menace",
  "TheSchnitzelSurvivor",
  "TheWaffle-Charger",
  "The_BobaTitan",
  "The_BurritoHandler",
  "The_CheeseMenace",
  "The_DumplingProwler",
  "The_FriesBeast",
  "The_IceCreamKnight",
  "The_MayoCrusader",
  "The_QuesoLancer",
  "The_TacoNinja",
  "The_TacoSniper",
  "The_TempuraGoblin",
  "The_WaffleMaster",
  "TofuBandit937",
  "TofuInvoker166",
  "TofuLightScout",
  "TofuPaladin429",
  "TofuTitan382",
  "TofuUltraBlaster",
  "TunaDealer",
  "TunaGoblin435",
  "TunaGrinder687",
  "TunaKing",
  "TunaMissDuelist",
  "TunaMrDuelist",
  "TunaRider406",
  "UltraBoba_Slinger",
  "UltraBrownie-Mancer",
  "UltraBurger_Dealer",
  "UltraCandy_Paladin",
  "UltraChai_Fury",
  "UltraCupcake_Invoker",
  "UltraFries-Champ",
  "UltraGinSlinger",
  "UltraHotdog_Boss",
  "UltraIceCreamSurvivor",
  "UltraJelly-Boss",
  "UltraMacaronGrinder",
  "UltraMatcha-Seeker",
  "UltraNoodle-Scout",
  "UltraNoodleFury",
  "UltraPancakeBoss",
  "UltraPancake_Blaster",
  "UltraPaneerMonk",
  "UltraPhoKing",
  "UltraRice_Queen",
  "UltraSoda-Lancer",
  "UltraSodaMonk",
  "UltraTempuraTank",
  "UltraTunaChallenger",
  "Ultra_BurgerBandit",
  "Ultra_CheeseBandit",
  "Ultra_GarlicSniper",
  "Ultra_GinMancer",
  "Ultra_GravyGoblin",
  "Ultra_JellyChallenger",
  "Ultra_MacaronTank",
  "Ultra_MilkshakeWarden",
  "Ultra_NachoHero",
  "Ultra_PickleGoblin",
  "Ultra_PizzaSniper",
  "Ultra_SalsaWarden",
  "WaffleBeast560",
  "WaffleDrNinja",
  "WaffleHyperBeast",
  "WaffleMaster159",
  "WaffleNeoRider",
  "WaffleScout527",
  "WaffleSilentProwler",
  "WaffleSurvivor280",
  "WaffleTitan374",
  "WhiskeyBigGrinder",
  "WhiskeyLightCrusader",
  "WhiskeyMissTank",
  "WhiskeyProwler720",
  "xCookieCrusader",
  "xDonut-Grinder",
  "xDonutCrusher",
  "xDosa-Paladin",
  "xDosa_Overlord",
  "xDumpling-Hunter",
  "xDumplingCharger",
  "xEspresso-Warden",
  "xFriesOverlord",
  "xFries_Ninja",
  "xHotdog_Warden",
  "xIceCreamNinja",
  "xMatchaBoss",
  "xMuffin_Commander",
  "xNugget_Rogue",
  "xPaneer-Buster",
  "xPaneer-Storm",
  "xPizza_Scout",
  "xPopcorn-Blaster",
  "xQueso_King",
  "xSamosa_Kid",
  "xWaffle_Mage",
  "x_ChaiTitan",
  "x_ChurroBeast",
  "x_CupcakeSurvivor",
  "x_KimchiKnight",
  "x_LatteCommander",
  "x_MilkshakeCrusader",
  "x_MilkshakeSurvivor",
  "x_PastaMenace",
  "x_WaffleBandit"
};

        List<int> higherScores = new List<int>(playersAbovePlayer); // these are the scores of players above the leaderboard
        List<int> lowerScores = new List<int>(rankup + playersBelowPlayer); //these are the scores of the players which will be below the player in the leaderboard.

        higherScores.Add(Random.Range(newScore + 1, newScore + increments/200));
        higherScores.Add(Random.Range(newScore + 1, newScore + increments/200));
        higherScores.Sort();
        higherScores.Reverse();

        TextMeshProUGUI rankText, scoreText, playerNameText;
        Image countryImage;

        int lowerPlayersCount = rankup + playersBelowPlayer;

        for (int i = 0; i < lowerPlayersCount; i++)
        {
            if (i < playersBelowPlayer)
            {
                lowerScores.Add((int)Mathf.Clamp(Random.Range(playerScore - increments/200, playerScore - 1), 0, Mathf.Infinity));
            }
            else
            {
                lowerScores.Add(Random.Range(playerScore + 1, newScore - 1));
            }
        }
        lowerScores.Sort();
        lowerScores.Reverse();

        int higherScoresPtr = 0;
        int lowerScoresPtr = 0;
        leaderboardItemPrefab.SetActive(true);
        for (int i = 0; i < numberOfBarsToSpawn; i++)
        {
            var item = Instantiate(leaderboardItemPrefab, leaderBoardListContainer);
            leaderboardSpawnList.Add(item);
            rankText = item.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            playerNameText = item.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            countryImage = item.transform.GetChild(1).GetChild(0).GetComponent<Image>();
            scoreText = item.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

            rankText.text = "#" + (currentRank).ToString();
            //if (currentRank <= 0) item.gameObject.SetActive(false);
            if (i == numberOfBarsToSpawn - 1 - playersBelowPlayer) currentRank++;
            currentRank++;
            playerNameText.text = leaderboardNames[Random.Range(0, leaderboardNames.Length)];
            countryImage.sprite = countryIcons[Random.Range(0, countryIcons.Length)];

            if (i < playersAbovePlayer)
            {
                scoreText.text = higherScores[higherScoresPtr].ToString();
                higherScoresPtr++;
            }
            else
            {
                scoreText.text = lowerScores[lowerScoresPtr].ToString();
                lowerScoresPtr++;
            }
        }

        leaderboardItemPrefab.SetActive(false);
        
        playerLeaderboardItem.transform.SetSiblingIndex(playerLeaderboardItem.transform.parent.childCount - 1 - playersBelowPlayer); // third last

        rankText = playerLeaderboardItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        scoreText = playerLeaderboardItem.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        rankText.text = "#" + playerRank.ToString();
        scoreText.text = playerScore.ToString();
        float startNormalizedPos = (2.25f / (float)(numberOfBarsToSpawn + 1));
        // float startNormalizedPos = GetNormalizedPos(numberOfBarsToSpawn-playersBelowPlayer,numberOfBarsToSpawn,playerLeaderboardItem.GetComponent<RectTransform>(),leaderboardScrollRect.GetComponent<RectTransform>());
        DOVirtual.DelayedCall(0.01f, () =>
        {
            leaderboardScrollRect.verticalNormalizedPosition = ExtensionMethods.Map(0, 0, 1, startNormalizedPos, 1);
            leaderBoardGO.SetActive(true);
        });

        int startIndex = playerLeaderboardItem.transform.GetSiblingIndex();
        int endIndex = 3;
        int currIdx = startIndex;

        int tempNewIdx;
        PlayerTotalScore = newScore;
        int startRank = playerRank;
        int endRank = startRank - rankup;
        PlayerRank = endRank;
        UpdateLeaderboardButtons(endRank);
        int startScore = playerScore;
        int endScore = newScore;

        //ANIMATION

        DOVirtual.DelayedCall(animationDelay, () =>
        {
            rankText = playerLeaderboardItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            scoreText = playerLeaderboardItem.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            float t = 0;
            DOTween.To(() => t, x => t = x, 1f, 2f).SetEase(Ease.InOutQuart).OnUpdate(() => 
            {
                leaderboardScrollRect.verticalNormalizedPosition = ExtensionMethods.Map(t, 0, 1, startNormalizedPos, 1);
                tempNewIdx = Mathf.RoundToInt(ExtensionMethods.Map(t, 0, 1, startIndex, endIndex));
                if (tempNewIdx != currIdx)
                {
                    currIdx = tempNewIdx;
                    //AudioManager.instance.PlayRankUp();
                    playerLeaderboardItem.transform.SetSiblingIndex(currIdx);
                }
                rankText.text = "#" + Mathf.RoundToInt(Mathf.Lerp(startRank, endRank, t)).ToString();
                scoreText.text = Mathf.RoundToInt(Mathf.Lerp(startScore, endScore, t)).ToString();
            }).OnComplete(() => 
            {
                t = 1;
                leaderboardScrollRect.verticalNormalizedPosition = ExtensionMethods.Map(t, 0, 1, startNormalizedPos, 1);
                tempNewIdx = Mathf.RoundToInt(ExtensionMethods.Map(t, 0, 1, startIndex, endIndex));
                if (tempNewIdx != currIdx)
                {
                    currIdx = tempNewIdx;
                    playerLeaderboardItem.transform.SetSiblingIndex(currIdx);
                }
                rankText.text = "#" + Mathf.RoundToInt(Mathf.Lerp(startRank, endRank, t)).ToString();
                scoreText.text = Mathf.RoundToInt(Mathf.Lerp(startScore, endScore, t)).ToString();


                rankText.text = "#" + endRank.ToString();
                scoreText.text = endScore.ToString();
                Transform _parent = playerLeaderboardItem.transform.parent;
                int count = _parent.childCount;
                int _startRank = endRank + 1;
                for (int i = 4; i < count; i++)
                {
                    var item = _parent.GetChild(i);
                    rankText = item.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    rankText.text = "#" + (_startRank).ToString();
                    _startRank++;
                }
            }).SetId(leaderboardTweenId);
        });
    }

    int leaderboardTweenId;

    public float GetNormalizedPos(int targetIndex, int totalElements, RectTransform elementRectTransform, RectTransform scrollRectViewport)
    {
        // Get the size of each element in the list
        float elementHeight = elementRectTransform.rect.height;

        // Get the size of the visible portion of the ScrollRect
        float visibleHeight = scrollRectViewport.rect.height;

        // Calculate the total height of all the elements in the list
        float totalHeight = elementHeight * totalElements;

        // Calculate the maximum scroll position that will center the target element
        float maxScrollPosition = totalHeight - visibleHeight;

        // Calculate the normalized position that will center the targe t element
        float centeredVerticalNormalizedPosition = (targetIndex * elementHeight + elementHeight / 2 - visibleHeight / 2) / maxScrollPosition;

        return centeredVerticalNormalizedPosition;
    }

    public void IncreaseRank(int increaseRankUp)
    {
        void SuccessCallbacK()
        {
            Debug.Log("Jump rank success callback");
            if (DOTween.IsTweening(leaderboardTweenId))
            {
                DOTween.Kill(leaderboardTweenId, true);
            }
            GameObject tempObj = new GameObject();
            //int increaseRankUp = 25;
            //Add Reward logic
            for (int i = contentObj.childCount - 1; i >= 0; i--)
            {
                if (i != 0 && i != 3)
                    contentObj.GetChild(i).SetParent(tempObj.transform);
            }

            Destroy(tempObj);

            leaderboardItemPrefab.SetActive(true);

            float inc = 100000000 / 10000;

            int increasedScore = Mathf.RoundToInt(inc * increaseRankUp);

            //Lock.instance.LockCount -= increasedScore; 

            if (PlayerRank - increaseRankUp < 1)
            {
                increaseRankUp = PlayerRank - 1;
            }
            
            //ProgressManager.instance.AddMoney(increasedScore);
            //BetManager.Instance.BncAmount += increasedScore;

            InitializeLeaderBoard(PlayerTotalScore, PlayerRank, PlayerTotalScore + increasedScore, increaseRankUp, 0);
        }

        SuccessCallbacK();
        //GameAnalyticsController.Miscellaneous.NewDesignEvent("rv:increase_rank");
        // HCSDKManager.INSTANCE.DisplayRV(HCSDKManager.RV_LOAD_NAME, () =>
        // {
        //     GameAnalyticsController.Miscellaneous.NewDesignEvent("rv:increase_rank");
        //     SuccessCallbacK();
        // });
    }
}
