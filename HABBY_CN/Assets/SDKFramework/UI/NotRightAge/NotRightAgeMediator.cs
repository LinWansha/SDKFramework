using SDKFramework.UI;

public class NotRightAgeMediator : UIMediator<NotRightAgeView>
{
   protected override void OnInit(NotRightAgeView view)
   {
      base.OnInit(view);
      view.btnSure.onClick.AddListener(Close);
   }
}