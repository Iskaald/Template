namespace Core.UI
{
    public class ProcessingWindow : BaseWindow<ProcessingWindowData>
    {
        private ProcessingWindowAnimator animator = new();
        
        public override bool IsInitialized { get; }

        protected override void Initialize()
        {
            WillHide += ProcessHide;
            
            animator.StartDOTweenAnimation(View.workingIcon.transform);
        }

        protected override void UpdateData()
        {
            animator.StartDOTweenAnimation(View.workingIcon.transform);
        }

        private void ProcessHide()
        {
            WillHide -= ProcessHide;
            animator.StopAnimation();
        }
    }
}