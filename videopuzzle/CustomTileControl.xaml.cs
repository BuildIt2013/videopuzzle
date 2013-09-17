using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace CustomTile
{
    public partial class CustomTileControl : UserControl
    {
        public CustomTileControl()
        {
            InitializeComponent();
            updateUI();
        }

        public void updateUI()
        {

        }

        public void updateUI(List<Image> images)
        {
            
        }
    }
}
