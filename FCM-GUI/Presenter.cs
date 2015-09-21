using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppLogic;

namespace FCM_GUI
{
    class Presenter
    {
        private readonly Logic _model;
        private readonly MainForm _view;

        public Presenter(Logic model,MainForm view)
        {
            _model = model;
            _view = view;
            _view.ActivateAnalysis += _view_ActivateAnalysis;
            _model.AnalysisMade += _model_AnalysisMade;
        }

        private void _model_AnalysisMade(object sender, ChartData e)
        {
            _view.SetText(e.Y);
            _view.SetChart(e.X, e.Y);
        }

        private void _view_ActivateAnalysis(object sender, int e)
        {
            _model.Analysis(e);
        }

    }
}
