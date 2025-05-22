namespace Core.UI
{
    public class ProcessingWindow : BaseWindow<ProcessingWindowView>
    {
        public override bool IsInitialized => true;

        protected override void Initialize()
        {
        }

        protected override void UpdateData()
        {
            View.StartAnimation();
        }

        protected override void OnHidden()
        {
            base.OnHidden();
            View.StopAnimation();
        }
    }
}