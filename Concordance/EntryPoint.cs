using Concordance.View;

namespace Concordance
{
    public class EntryPoint
    {
        private readonly IView _view;
        public EntryPoint(IView view)
        {
            _view = view;
        }
        public void Run()
        {
            _view.Show();
        }
    }
}
