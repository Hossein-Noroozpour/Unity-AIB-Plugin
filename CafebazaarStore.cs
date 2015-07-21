using UnityEngine;
using System.Collections;

public class CafebazaarStore : MonoBehaviour
{
    //private class OnIabPurchaseFinishedListener : AndroidJavaProxy
    //{
    //    public OnIabPurchaseFinishedListener() : base("com.hulixerian.cafebazaariab.IabHelper$OnIabPurchaseFinishedListener") { }
    //    public void onIabPurchaseFinished(AndroidJavaObject result, AndroidJavaObject purchase)
    //    {
    //        if (result.Call<bool>("isFailure"))
    //        {
    //            text += "Error purchasing.\n";
    //            return;
    //        }
    //        else if (purchase.Call<string>("getSku") == SKU_TEST_GOOD)
    //        {
    //        }
    //    }
    //}
    private class OnIabSetupFinishedListene : AndroidJavaProxy
    {
        public OnIabSetupFinishedListene() : base("com.hulixerian.cafebazaariab.IabHelper$OnIabSetupFinishedListener") { }
        public void onIabSetupFinished(AndroidJavaObject resultIabHelper)
        {
            text += "Unity: Setup finished.\n";
            if (!resultIabHelper.Call<bool>("isSuccess"))
            {
                text += "Unity: Problem setting up In-app Billing.\n";
            }
            //mHelper.Call("queryInventoryAsync", mGotInventoryListener);
        }
    }
    private class QueryInventoryFinishedListener : AndroidJavaProxy
    {
        public QueryInventoryFinishedListener() : base("com.hulixerian.cafebazaariab.IabHelper$QueryInventoryFinishedListener") { }
        public void onQueryInventoryFinished(AndroidJavaObject result, AndroidJavaObject inventory)
        {
            text += "Unity: Query inventory finished.\n";
            if (result.Call<bool>("isFailure"))
            {
                text += "Unity: Failed to query inventory.\n";
                return;
            }
            else
            {
                text += "Unity: Query inventory was successful.\n";
                mIsPremium = inventory.Call<bool>("hasPurchase", SKU_TEST_GOOD);
                text += "Unity: User is " + (mIsPremium ? "PREMIUM" : "NOT PREMIUM");
            }
            text += "Unity: Initial inventory query finished; enabling main UI.\n";
        }
    }
    private static string SKU_TEST_GOOD = "test-good";
    private static bool mIsPremium;
    //private static int RC_REQUEST;
    private static AndroidJavaObject mHelper;
    private static AndroidJavaClass cMainActivity;
    private static AndroidJavaObject unityActivity;
    private static QueryInventoryFinishedListener mGotInventoryListener;
    //private static OnIabPurchaseFinishedListener mPurchaseFinishedListener;
    private static OnIabSetupFinishedListene mOnIabSetupFinishedListene;
    private static string text = "";
    private static string base64EncodedPublicKey = "MIHNMA0GCSqGSIb3DQEBAQUAA4G7ADCBtwKBrwDVpDrj0EirMtqhLlbg" +
				"Dl2qva+3IFx+9eqgYuKehxDi3QQttIc9OYsC27blbaozZs6bUw0XZnGG/ROdNj0yM/7Mcnm8OgQ33G8c" +
				"owRfKIz0S1SwIODcfSBo/BopZAZfZmFRm4e7+qIn9VeWIvhyS2GPlHpMjNm98ktANU3636tigV078cW6" +
				"/cAgQDDNyJy7BlzJjrOCmDcaERI0m/w5cHs/7PYAwET18wHkFfmOr78CAwEAAQ==";
    void Start()
    {
        mGotInventoryListener = new QueryInventoryFinishedListener();
        //mPurchaseFinishedListener = new OnIabPurchaseFinishedListener();
        mOnIabSetupFinishedListene = new OnIabSetupFinishedListene();
        cMainActivity = new AndroidJavaClass("com.hulixerian.cafebazaariab.MainActivity");
        unityActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        //cMainActivity.CallStatic("initialize", unityActivity, mGotInventoryListener, mOnIabSetupFinishedListene);
        text += "initialized.\n";
    }
    void Update()
    {

    }
    void OnGUI()
    {

        if (GUI.Button(new Rect(1, 1, 60, 20), "Initialize"))
        {
            cMainActivity.CallStatic("initializeIAB", unityActivity);
            //text += "\nJava: " + cMainActivity.GetStatic<string>("print");
        }
        if (GUI.Button(new Rect(62, 1, 40, 20), "Start"))
        {
            cMainActivity.CallStatic("startIAB", mOnIabSetupFinishedListene);
            //text += "\nJava: " + cMainActivity.GetStatic<string>("print");
        }
        if (GUI.Button(new Rect(103, 1, 50, 20), "Query"))
        {
            cMainActivity.CallStatic("queryInventory");
            text += "\nJava: " + cMainActivity.GetStatic<string>("print");
        }
        if (GUI.Button(new Rect(154, 1, 70, 20), "Purchase"))
        {
            cMainActivity.CallStatic("purchase");
            text += "JAVA: " + cMainActivity.GetStatic<string>("print");
        }
        if (GUI.Button(new Rect(225, 1, 70, 20), "Consume"))
        {
            cMainActivity.CallStatic("queryIAB", unityActivity);
            //text = cMainActivity.GetStatic<string>("print");
        }
        GUI.TextArea(new Rect(1, 22, 450, 500), text);
    }
}
