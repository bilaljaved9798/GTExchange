using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace globaltraders
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Timeline.DesiredFrameRateProperty.OverrideMetadata(typeof(Timeline),
   new FrameworkPropertyMetadata { DefaultValue = 10 });
            //Try to improve performances
            int renderingTier = (RenderCapability.Tier >> 16);
            if (renderingTier < 2)
            {
                RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
            }
        }
    }
}
