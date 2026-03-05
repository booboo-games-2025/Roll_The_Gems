using UnityEngine;

public class MyConstants : MonoBehaviour
{
    public const string BALL_UPGRADE_DATA = "upgrade_data";
    public const string POWERUPS_UPGRADE_DATA = "powerups_data";
    public const string COIN_COUNT = "coin_count";
    public const string PERMANENT_COIN_COUNT = "permanent_coin_count";
    public const string RING_LEVEL = "ring_level";
    
    // string values for common power ups

    public const string RING_HEALTH = "ring_health";
    public const string ALL_INCOME = "all_income";
    
    // string values for achievements data

    public const string EARNED_INCOME = "earned_income";
    public const string CREATE_BALL = "create_ball";
    public const string DESTROY_RINGS = "destroy_ball";
    public const string UPGRADE_TIMES = "upgrade_times";
    public const string POWERUPS_TIMES = "powerups_times";
    public const string CRITICAL_TIMES = "critical_times";
    public const string GAMEPLAY_TIME = "game_time";
    public const string OFFLINE_INCOME_COUNT = "offline_income_count";
    
    // Ftue player prefs keys
    
    public const string StartFtueCompleted = "start_ftue_completed";
    public const string AchievementFtueCompleted = "achievement_ftue_completed";
    
    // IAP playersprefs save keys
    
    public const string INCOME_BUNDLE_PURCHASED = "income_bundle";
    public const string SPEED_POWER_BUNDLE_PURCHASED = "speed_power_bundle";
    public const string MEGA_UPGRADE_BUNDEL_PURCHASED = "mega_upgrade_bundle";
    
    
    /// <summary>
    /// GA EVENTS
    /// </summary>
    
    // Rv
    public const string FLOATING_RV = "rv:floating_coin";
    public const string TWOX_INCOME_RV = "rv:twox_income";
    public const string TWOX_CREATION_SPEED_RV = "rv:twox_creation_speed";
    public const string TWOX_BALL_SPEED_RV = "rv:twox_ball_speed";
    public const string TWOX_RING_DAMAGE_RV = "rv:twox_ring_damage";
    public const string INFINITE_DURABILITY_RV = "rv:infinite_durability";
    public const string RANK_INCREASED_RV = "rv:rank_increased";
    public const string OFFLINE_INCOME_DOUBLE_RV = "rv:offline_income_double";
    public const string CRITICAL_POWER_UPGRADE_RV = "rv:critical_power_upgrade";
    public const string SPEED_UPGRADE_RV = "rv:speed_upgrade";
    public const string CREATION_TIME_UPGRADE_RV = "rv:creation_time_upgrade";
    public const string DURABILITY_UPGRADE_RV = "rv:durability_upgrade";
    
    // ftue
    public const string GA_STARTING_FTUE = "ftue:first_tutorial_completed";
    public const string GA_ACHIEVEMENT_FTUE  =  "ftue:achievement_tutorial_completed";
    
    // IAP GA Events

    public const string GA_INCOME_PACK_SUCCESS = "iap_purchase:income_pack";
    public const string GA_SPEED_POWER_PACK_SUCCESS = "iap_purchase:speed_power_pack";
    public const string GA_MEGA_PACK_SUCCESS = "iap_purchase:mega_upgrade_pack";

    public const string GA_COIN_PACK_1_SUCCESS = "iap_purchase:coin_pack_1";
    public const string GA_COIN_PACK_2_SUCCESS = "iap_purchase:coin_pack_2";
    public const string GA_COIN_PACK_3_SUCCESS = "iap_purchase:coin_pack_3";
    
    
    /// <summary>
    /// 
    /// </summary>
    
    
    // IAP keys

    public const string COINS_PACK_1 = "brb_coin_pack_1";
    public const string COINS_PACK_2 = "brb_coin_pack_2";
    public const string COINS_PACK_3 = "brb_coin_pack_3";
    public const string INCOME_BUNDLE_PACK = "brb_income_bundle_pack";
    public const string SPEED_BUNDLE_PACK = "brb_speed_power_bundle_pack";
    public const string MEGA_UPGRADE_BUNDLE_PACK = "brb_mega_upgrade_bundle_pack";
    
    public const string COINS_PACK_1_PRICE = "2.99";
    public const string COINS_PACK_2_PRICE = "4.99";
    public const string COINS_PACK_3_PRICE = "9.99";
    public const string INCOME_BUNDLE_PACK_PRICE = "5.99";
    public const string SPEED_BUNDLE_PACK_PRICE = "2.99";
    public const string MEGA_UPGRADE_BUNDLE_PACK_PRICE = "4.99";
}
