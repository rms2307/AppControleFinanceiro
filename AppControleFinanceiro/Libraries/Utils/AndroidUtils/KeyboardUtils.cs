using Microsoft.Maui.Platform;

namespace AppControleFinanceiro.Libraries.Utils.AndroidUtils
{
    public class KeyboardUtils
    {
        public static void HideKeyboardAndroid()
        {
#if ANDROID       

            if (Platform.CurrentActivity.CurrentFocus != null)
            {
                Platform.CurrentActivity.HideKeyboard(Platform.CurrentActivity.CurrentFocus);
            }
#endif
        }
    }
}
