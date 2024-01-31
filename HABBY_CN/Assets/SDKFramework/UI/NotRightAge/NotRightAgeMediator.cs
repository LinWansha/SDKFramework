using SDKFramework.UI;

public class NotRightAgeMediator : UIMediator<NotRightAgeView>
{
   protected override void OnShow(object arg)
   {
      base.OnShow(arg);
      view.btnSure.onClick.AddListener(Close);
   }

   protected override void OnHide()
   {
      view.btnSure.onClick.RemoveListener(Close);
      base.OnHide();
   }
}