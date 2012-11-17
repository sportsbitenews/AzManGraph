using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace AzManGraph
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow, INotifyPropertyChanged
    {
        public ObservableCollection<string> LayoutAlgorithmChoices
        {
            get { return mLayoutAlgorithmChoices; }
            set
            {
                if (Equals(mLayoutAlgorithmChoices, value))
                {
                    return;
                }

                mLayoutAlgorithmChoices = value;
                OnPropertyChanged(() => LayoutAlgorithmChoices);
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { mPropertyChanged += value; }
            remove { mPropertyChanged -= value; }
        }


        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadAzManFile()
        {
            var dialog
                = new OpenFileDialog
                {
                    Filter = "Xml Documents (*.xml)|*.xml|All Files (*.*)|*.*"
                };
            if (dialog.ShowDialog() == true)
            {
                var loader = new Loader();
                loader.LoadFromFile(dialog.FileName);
                graphLayout.Graph = loader.Graph;
            }

        }

        /// <summary>
        /// Copies a UI element to the clipboard as an image.
        /// </summary>
        /// <param name="element">The element to copy.</param>
        public static void CopyUIElementToClipboard(FrameworkElement element)
        {
            var bounds = VisualTreeHelper.GetContentBounds(element);

            int imageWidth = (int)(element.ActualWidth);
            int imageHeight = (int)(element.ActualHeight);

            var image = new RenderTargetBitmap(imageWidth, imageHeight, 96.0, 96.0, PixelFormats.Pbgra32);
            image.Render(element);
            Clipboard.SetImage(image);
        }

        /// <summary>
        /// Trigger the PropertyChanged event
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            var handlers = mPropertyChanged;
            if (handlers != null)
            {
                var args = new PropertyChangedEventArgs(propertyName);
                handlers(this, args);
            }
        }

        /// <summary>
        /// Trigger the PropertyChanged event
        /// </summary>
        /// <param name="propertyExpression">
        /// Expression of the form "() => Property" specifying the property that changed.</param>
        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var body = (MemberExpression)propertyExpression.Body;
            OnPropertyChanged(body.Member.Name);
        }

        private void RibbonButton_Click_1(object sender, RoutedEventArgs e)
        {
            CopyUIElementToClipboard(graphLayout);
        }

        /// <summary>
        /// Storage for the LayoutAlgorithmChoices property
        /// </summary>
        private ObservableCollection<string> mLayoutAlgorithmChoices = new ObservableCollection<string>();

        /// <summary>
        /// Storage for the PropertyChanged event.
        /// </summary>
        private PropertyChangedEventHandler mPropertyChanged;

        private void RibbonApplicationMenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            LoadAzManFile();
        }

        private void RibbonApplicationMenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
