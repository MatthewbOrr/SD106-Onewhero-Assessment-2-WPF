using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SD106_Onewhero_Assessment_2.Helpers;
using SD106_Onewhero_Assessment_2.Model;

namespace SD106_Onewhero_Assessment_2.View
{

    public partial class VisitorDashboardPage : Page
    {

        public VisitorDashboardPage()
        {
            InitializeComponent();
        }
        public VisitorDashboardPage(Model.User user)
        {
            InitializeComponent();
        }
    }
}
