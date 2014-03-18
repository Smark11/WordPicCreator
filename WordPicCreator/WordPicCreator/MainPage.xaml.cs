using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WordPicCreator.Resources;
using Microsoft.Xna.Framework.Media;
using System.IO;
using Microsoft.Phone;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Microsoft.Xna.Framework.Input;
using System.IO.IsolatedStorage;
using System.Windows.Resources;
using Common.IsolatedStoreage;
using System.Windows.Input;

namespace WordPicCreator
{
    public partial class MainPage : PhoneApplicationPage
    {
        private MediaLibrary _mediaLibrary = new MediaLibrary();
        private string FILE_NUMBER = "FILENUMBER";
        private double _screenWidth;
        private double _screenHeight;
        private double _originX = 0;
        private double _originY = 0;
        private int _widthOfPhoto;
        private int _heightOfPhoto;
        private bool _userHasSavedChanges = false;

        private double _leftOfTextBox;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            _screenWidth = System.Windows.Application.Current.Host.Content.ActualWidth;
            _screenHeight = System.Windows.Application.Current.Host.Content.ActualHeight;
            _deleteTextBlockButton.IsEnabled = false;
            _saveButton.IsEnabled = false;
            _orientation = PageOrientation.Portrait;

            // Sample code to localize the ApplicationBar
            BuildLocalizedApplicationBar();

            //Img.Width = _screenWidth;
            //Img.Height = _screenHeight;
            //CnVas.Height = _screenHeight;
            //CnVas.Width = _screenWidth;

            LoadPhotosFromLibrary();
            Img.Tap += Img_Tap;
            //canvas.Tap += CnVas_Tap;
            LayoutRoot.Tap += LayoutRoot_Tap;


            //CnVas.ManipulationStarted += CnVas_ManipulationStarted;
            //CnVas.ManipulationDelta += CnVas_ManipulationDelta;
            //Img.ManipulationStarted += Img_ManipulationStarted;
            //Img.ManipulationDelta += Img_ManipulationDelta;
        }

        void LayoutRoot_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Point tappedAt = e.GetPosition(null);

            double percentageY = tappedAt.Y / LayoutRoot.ActualHeight;
            double percentageX = tappedAt.X / LayoutRoot.ActualWidth;

            double width = canvas.Width;
            double height = canvas.Height;

            if (_textBoxesOnScreen.Count() == 0)
            {
                TextBox txtbox = GetNewTextBox();

                _originX = _centerOfViewPort.X;
                _originY = _centerOfViewPort.Y;

                Canvas.SetLeft(txtbox, _originX);
                Canvas.SetTop(txtbox, _originY);


                txtbox.KeyDown += txtbox_KeyDown;
                _textBoxesOnScreen.Add(txtbox);
                canvas.Children.Add(txtbox);

                txtbox.Focus();
            }


        }

        private void Img_Loaded()
        {
            _bitmap = (BitmapImage)Img.Source;

            // Set scale to the minimum, and then save it. 
            _scale = 0;
            CoerceScale(true);
            _scale = _coercedScale;

            ResizeImage(true);
        }

        #region try1

        void Img_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            var transform = (CompositeTransform)Img.RenderTransform;

            // pan
            transform.TranslateX = _translationX + e.CumulativeManipulation.Translation.X;
            transform.TranslateY = _translationY + e.CumulativeManipulation.Translation.Y;

            // zoom
            if (e.PinchManipulation != null)
            {
                transform.CenterX = e.PinchManipulation.Original.Center.X;
                transform.CenterY = e.PinchManipulation.Original.Center.Y;

                transform.ScaleX = _scaleX * e.PinchManipulation.CumulativeScale;
                transform.ScaleY = _scaleY * e.PinchManipulation.CumulativeScale;
            }

        }

        public static double angleBetween2Lines(PinchContactPoints line1, PinchContactPoints line2)
        {
            if (line1 != null && line2 != null)
            {
                double angle1 = Math.Atan2(line1.PrimaryContact.Y - line1.SecondaryContact.Y,
                                           line1.PrimaryContact.X - line1.SecondaryContact.X);
                double angle2 = Math.Atan2(line2.PrimaryContact.Y - line2.SecondaryContact.Y,
                                           line2.PrimaryContact.X - line2.SecondaryContact.X);
                return (angle1 - angle2) * 180 / Math.PI;
            }
            else { return 0.0; }
        }

        void Img_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            try
            {
                // the user has started manipulating the screen, set starting points
                var transform = (CompositeTransform)Img.RenderTransform;
                _scaleX = transform.ScaleX;
                _scaleY = transform.ScaleY;
                _translationX = transform.TranslateX;
                _translationY = transform.TranslateY;

            }
            catch (Exception ex)
            {

            }
        }

        private double _translationX;
        private double _translationY;
        private double _scaleX;
        private double _scaleY;

        void CnVas_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {

        }

        void CnVas_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {

        }

        #endregion try1

        #region newcode

        const double MaxScale = 100;

        double _scale = 1.0;
        double _minScale;
        double _coercedScale;
        double _originalScale;

        Size _viewportSize;
        bool _pinching;
        Point _screenMidpoint;
        Point _relativeMidpoint;

        BitmapImage _bitmap;

        /// <summary> 
        /// Either the user has manipulated the image or the size of the viewport has changed. We only 
        /// care about the size. 
        /// </summary> 
        void viewport_ViewportChanged(object sender, System.Windows.Controls.Primitives.ViewportChangedEventArgs e)
        {
            Size newSize = new Size(viewport.Viewport.Width, viewport.Viewport.Height);
            if (newSize != _viewportSize)
            {
                _viewportSize = newSize;
                CoerceScale(true);
                ResizeImage(false);
            }
        }

        /// <summary> 
        /// Handler for the ManipulationStarted event. Set initial state in case 
        /// it becomes a pinch later. 
        /// </summary> 
        void OnManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            _pinching = false;
            _originalScale = _scale;
        }

        /// <summary> 
        /// Handler for the ManipulationDelta event. It may or may not be a pinch. If it is not a  
        /// pinch, the ViewportControl will take care of it. 
        /// </summary> 
        /// <param name="sender"></param> 
        /// <param name="e"></param> 
        void OnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (e.PinchManipulation != null)
            {
                e.Handled = true;

                if (!_pinching)
                {
                    _pinching = true;
                    Point center = e.PinchManipulation.Original.Center;
                    _relativeMidpoint = new Point(center.X / Img.ActualWidth, center.Y / Img.ActualHeight);

                    var xform = Img.TransformToVisual(viewport);
                    _screenMidpoint = xform.Transform(center);
                }

                _scale = _originalScale * e.PinchManipulation.CumulativeScale;

                CoerceScale(false);
                ResizeImage(false);
            }
            else if (_pinching)
            {
                _pinching = false;
                _originalScale = _scale = _coercedScale;
            }
        }

        /// <summary> 
        /// The manipulation has completed (no touch points anymore) so reset state. 
        /// </summary> 
        void OnManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            _pinching = false;
            _scale = _coercedScale;
        }


        /// <summary> 
        /// When a new image is opened, set its initial scale. 
        /// </summary> 
        void OnImageOpened(object sender, RoutedEventArgs e)
        {
            _bitmap = (BitmapImage)Img.Source;

            // Set scale to the minimum, and then save it. 
            _scale = 0;
            CoerceScale(true);
            _scale = _coercedScale;

            ResizeImage(true);
        }

        Point _centerOfViewPort = new Point();

        /// <summary> 
        /// Adjust the size of the image according to the coerced scale factor. Optionally 
        /// center the image, otherwise, try to keep the original midpoint of the pinch 
        /// in the same spot on the screen regardless of the scale. 
        /// </summary> 
        /// <param name="center"></param> 
        void ResizeImage(bool center)
        {
            if (_coercedScale != 0 && _bitmap != null)
            {
                double newWidth = canvas.Width = Math.Round(_bitmap.PixelWidth * _coercedScale);
                double newHeight = canvas.Height = Math.Round(_bitmap.PixelHeight * _coercedScale);

                xform.ScaleX = xform.ScaleY = _coercedScale;

                viewport.Bounds = new Rect(0, 0, newWidth, newHeight);

                if (center)
                {
                    viewport.SetViewportOrigin(
                        new Point(
                            Math.Round((newWidth - viewport.ActualWidth) / 2),
                            Math.Round((newHeight - viewport.ActualHeight) / 2)
                            ));
                }
                else
                {
                    Point newImgMid = new Point(newWidth * _relativeMidpoint.X, newHeight * _relativeMidpoint.Y);
                    Point origin = new Point(newImgMid.X - _screenMidpoint.X, newImgMid.Y - _screenMidpoint.Y);
                    viewport.SetViewportOrigin(origin);
                }
            }
        }

        /// <summary> 
        /// Coerce the scale into being within the proper range. Optionally compute the constraints  
        /// on the scale so that it will always fill the entire screen and will never get too big  
        /// to be contained in a hardware surface. 
        /// </summary> 
        /// <param name="recompute">Will recompute the min max scale if true.</param> 
        void CoerceScale(bool recompute)
        {
            if (recompute && _bitmap != null && viewport != null)
            {
                // Calculate the minimum scale to fit the viewport 
                double minX = viewport.ActualWidth / _bitmap.PixelWidth;
                double minY = viewport.ActualHeight / _bitmap.PixelHeight;

                _minScale = Math.Min(minX, minY);
            }

            _coercedScale = Math.Min(MaxScale, Math.Max(_scale, _minScale));

        }

        #endregion newcode

        void CnVas_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (_textBoxesOnScreen.Count() == 0)
            {
                Point tappedAt = e.GetPosition(null);
                _leftOfTextBox = (_orientation == PageOrientation.Portrait) ? tappedAt.X : tappedAt.Y;

                TextBox txtbox = GetNewTextBox();

                if (_orientation == PageOrientation.Portrait)
                {
                    _originX = tappedAt.X;
                    _originY = tappedAt.Y;
                    Canvas.SetLeft(txtbox, tappedAt.X);
                    Canvas.SetTop(txtbox, tappedAt.Y);
                }
                else
                {
                    _originX = tappedAt.X;
                    _originY = tappedAt.Y;
                    Canvas.SetLeft(txtbox, _originX);
                    Canvas.SetTop(txtbox, _originY);
                }

                txtbox.KeyDown += txtbox_KeyDown;
                _textBoxesOnScreen.Add(txtbox);
                canvas.Children.Add(txtbox);

                txtbox.Focus();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (_textBoxesOnScreen != null)
            {
                if (_textBoxesOnScreen.Count() > 0)
                {
                    foreach (var row in _textBoxesOnScreen)
                    {
                        row.FontFamily = GetFontFamily();
                        row.FontSize = GetFontSize();
                        row.Foreground = GetFontColor();
                        row.FontWeight = GetFontWeight();
                        row.FontStyle = GetFontStyle();
                    }
                }
            }
        }

        List<TextBox> _textBoxesOnScreen = new List<TextBox>();

        void Img_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        private TextBox GetNewTextBox()
        {
            TextBox returnValue = new TextBox();

            returnValue.FontFamily = GetFontFamily();
            returnValue.FontSize = GetFontSize();
            returnValue.Foreground = GetFontColor();
            returnValue.FontWeight = GetFontWeight();
            returnValue.FontStyle = GetFontStyle();
            returnValue.ManipulationDelta += returnValue_ManipulationDelta;
            returnValue.TextWrapping = TextWrapping.Wrap;
            returnValue.MaxWidth = _orientation == PageOrientation.Portrait ? _screenWidth - _leftOfTextBox : _screenHeight - _leftOfTextBox;
            returnValue.LostFocus += returnValue_LostFocus;
            returnValue.GotFocus += returnValue_GotFocus;
            returnValue.Name = "Caption";

            return returnValue;
        }

        private void returnValue_GotFocus(object sender, RoutedEventArgs e)
        {
            _saveButton.IsEnabled = false;
        }

        private void returnValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_textBoxesOnScreen.Count() > 0)
            {
                if (_textBoxesOnScreen[0].Text == null)
                {
                    ClearTextBoxes();
                }
                else if (_textBoxesOnScreen[0].Text == string.Empty)
                {
                    ClearTextBoxes();
                }
                else
                {
                    _saveButton.IsEnabled = true;
                }
            }
        }

        private void txtbox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.Focus();
            }
            else
            {
                _deleteTextBlockButton.IsEnabled = true;
            }
        }

        void returnValue_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            Point manOrigin = e.ManipulationOrigin;

            Canvas.SetLeft(_textBoxesOnScreen[0], _originX + manOrigin.X);
            Canvas.SetTop(_textBoxesOnScreen[0], _originY + manOrigin.Y);

            _originX = _originX + manOrigin.X;
            _originY = _originY + manOrigin.Y;
        }

        private bool _imageMovingMode = false;

        #region Styling

        public FontFamily GetFontFamily()
        {
            return new FontFamily(IS.GetSettingStringValue(ISKeys.FONT));
        }

        public double GetFontSize()
        {
            return Convert.ToDouble(IS.GetSettingStringValue(ISKeys.FONTSIZE));
        }

        public SolidColorBrush GetFontColor()
        {
            return ISKeys.GetBrushFromString(IS.GetSettingStringValue(ISKeys.FONTCOLOR));
        }

        public FontWeight GetFontWeight()
        {
            if ((bool)IS.GetSetting(ISKeys.BOLD))
            {
                return FontWeights.Bold;
            }
            else
            {
                return FontWeights.Normal;
            }
        }

        public FontStyle GetFontStyle()
        {
            if ((bool)IS.GetSetting(ISKeys.ITALIC))
            {
                return FontStyles.Italic;
            }
            else
            {
                return FontStyles.Normal;
            }
        }

        #endregion Styling

        private void LoadPhotosFromLibrary()
        {
            PhotoChooserTask t = new PhotoChooserTask();
            t.Show();
            t.Completed += t_Completed;
        }

        private void t_Completed(object sender, PhotoResult e)
        {
            if (e.ChosenPhoto != null)
            {
                BitmapImage image = new BitmapImage();
                image.SetSource(e.ChosenPhoto);

                _widthOfPhoto = image.PixelWidth;
                _heightOfPhoto = image.PixelHeight;

                Img.Source = image;
            }

            Img_Loaded();
        }

        /// <summary>
        /// Gets the new file name to save
        /// </summary>
        private string GetFileName()
        {
            string returnValue = "WordPic";

            if (IS.GetSetting(FILE_NUMBER) == null)
            {
                returnValue = returnValue + 1;
            }
            else
            {
                returnValue = returnValue + ((int)IS.GetSetting(FILE_NUMBER) + 1);
                IS.SaveSetting(FILE_NUMBER, (int)IS.GetSetting(FILE_NUMBER) + 1);
            }

            return returnValue;
        }

        ApplicationBarIconButton _deleteTextBlockButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/delete.png", UriKind.Relative));
        ApplicationBarIconButton _saveButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/save.png", UriKind.Relative));

        // Sample code for building a localized ApplicationBar
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            ApplicationBar.Mode = ApplicationBarMode.Default;
            ApplicationBar.Opacity = 1.0;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;

            // Create a new button and set the text value to the localized string from AppResources.

            _saveButton.Click += saveButton_Click;
            _saveButton.Text = "Save";
            ApplicationBar.Buttons.Add(_saveButton);

            ApplicationBarIconButton selectNewPictureButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/new.png", UriKind.Relative));
            selectNewPictureButton.Text = "New Pic";
            selectNewPictureButton.Click += selectNewPictureButton_Click;
            ApplicationBar.Buttons.Add(selectNewPictureButton);

            ApplicationBarIconButton settingsButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/feature.settings.png", UriKind.Relative));
            settingsButton.Text = "Settings";
            settingsButton.Click += settingsButton_Click;
            ApplicationBar.Buttons.Add(settingsButton);

            _deleteTextBlockButton.Text = "Delete Text";
            _deleteTextBlockButton.Click += deleteTextBlockButton_Click;
            ApplicationBar.Buttons.Add(_deleteTextBlockButton);


            ApplicationBarMenuItem rateMenuItem = new ApplicationBarMenuItem("Rate");
            rateMenuItem.Click += rateButton_Click;
            ApplicationBar.MenuItems.Add(rateMenuItem);

            // Create a new menu item with the localized string from AppResources.
            ApplicationBarMenuItem moreAppsMenuItem = new ApplicationBarMenuItem("More Apps From KLB");
            moreAppsMenuItem.Click += moreAppsMenuItem_Click;
            ApplicationBar.MenuItems.Add(moreAppsMenuItem);
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        void moreAppsMenuItem_Click(object sender, EventArgs e)
        {
            MarketplaceSearchTask marketplaceSearchTask = new MarketplaceSearchTask();

            marketplaceSearchTask.SearchTerms = "KLBCreations";
            marketplaceSearchTask.Show();
        }

        void rateButton_Click(object sender, EventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }

        void deleteTextBlockButton_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
        }

        void saveButton_Click(object sender, EventArgs e)
        {
            SaveChanges();
        }

        private void SaveChanges()
        {
            this.Focus();

            var bitmap = new WriteableBitmap(Convert.ToInt32(Img.Width), Convert.ToInt32(Img.Height));
            //bitmap.Render(CnVas, null);
            bitmap.Invalidate();

            String tempJPEG = "logo.jpg";

            //Create virtual store and file stream. Check for duplicate tempJPEG files.
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                try
                {
                    if (myIsolatedStorage.FileExists(tempJPEG))
                    {
                        myIsolatedStorage.DeleteFile(tempJPEG);
                    }
                }
                catch (Exception ex)
                {

                }

                IsolatedStorageFileStream fileStream = myIsolatedStorage.CreateFile(tempJPEG);

                // Encode WriteableBitmap object to a JPEG stream.
                Extensions.SaveJpeg(bitmap, fileStream, bitmap.PixelWidth, bitmap.PixelHeight, 0, 100);

                fileStream.Close();
                fileStream.Dispose();

                IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile(tempJPEG, FileMode.Open, FileAccess.Read);

                try
                {
                    _mediaLibrary.SavePictureToCameraRoll(GetFileName(), stream);
                }
                catch (Exception ex)
                {

                }

                stream.Close();
                stream.Dispose();

            }

            _userHasSavedChanges = true;
        }

        void selectNewPictureButton_Click(object sender, EventArgs e)
        {
            if (!_userHasSavedChanges)
            {
                MessageBoxResult rslt = MessageBox.Show("Do you wish to save your changes?", "Save Changes?", MessageBoxButton.OKCancel);

                if (rslt == MessageBoxResult.OK)
                {
                    SaveChanges();
                }
            }

            _deleteTextBlockButton.IsEnabled = false;
            _saveButton.IsEnabled = false;
            _userHasSavedChanges = false;

            ClearTextBoxes();

            LoadPhotosFromLibrary();
        }

        private void ClearTextBoxes()
        {
            //if (CnVas.Children.Count() > 0)
            //{
            //    for (int i = 0; i <= CnVas.Children.Count() - 1; i++)
            //    {
            //        TextBox t = CnVas.Children[i] as TextBox;

            //        if (t != null)
            //        {
            //            CnVas.Children.Remove(CnVas.Children[i]);
            //        }
            //    }
            //}

            _textBoxesOnScreen.Clear();
            _deleteTextBlockButton.IsEnabled = false;
            _saveButton.IsEnabled = false;
        }

        private PageOrientation _orientation;

        private void PhoneOrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            if (e.Orientation == PageOrientation.Landscape || e.Orientation == PageOrientation.LandscapeLeft || e.Orientation == PageOrientation.LandscapeRight)
            {
                Img.Width = _screenHeight;
                Img.Height = _screenWidth;

                _orientation = PageOrientation.Landscape;
            }
            else
            {
                Img.Width = _screenWidth;
                Img.Height = _screenHeight;

                _orientation = PageOrientation.Portrait;
            }
        }
    }
}