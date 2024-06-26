﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using CapstoneGroup2.Desktop.Data;
using CapstoneGroup2.Desktop.Library.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CapstoneGroup2.Desktop
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddSourceDialog : ContentDialog
    {
        #region Data members

        private static MessageDialog FileTypeDialog = new MessageDialog("Please select a file type.");

        private static MessageDialog FileLinkDialog = new MessageDialog("Please enter a link.");

        private static MessageDialog FileUploadDialog = new MessageDialog("Please select a file to upload.");

        private StorageFile storageFile;

        private bool? isPdf;

        #endregion

        #region Properties

        public Source NewSource { get; set; }

        #endregion

        #region Constructors

        public AddSourceDialog()
        {
            this.InitializeComponent();
            this.AttachEventListeners();
            IsPrimaryButtonEnabled = false;
            this.sourceIsLinkCheckBox.IsChecked = true;
        }

        #endregion

        #region Methods
        private void AttachEventListeners()
        {
            this.sourceNameTextBox.TextChanged += this.SourceNameTextBox_TextChanged;
            this.sourceAccessedDatePicker.DateChanged += this.DateTimePicker_DateChanged;
        }

        private void SourceNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = this.sourceNameTextBox.Text.Trim();
            var dateValid = this.sourceAccessedDatePicker.Date.Year >= 2018 ;
            if (text.Length == 0 || dateValid)
            {
                IsPrimaryButtonEnabled = false;
            }
            else
            {
                IsPrimaryButtonEnabled = true;
            }
        }

        private void DateTimePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            string text = this.sourceNameTextBox.Text.Trim();
            DatePicker dateTimePicker = (DatePicker)sender;
            DateTimeOffset selectedDate = dateTimePicker.Date;
            IsPrimaryButtonEnabled = selectedDate.Year >= 2018 && text.Length != 0;
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender,
            ContentDialogButtonClickEventArgs args)
        {
            var deferral = args.GetDeferral();
            args.Cancel = await this.createSource();
            deferral.Complete();
        }

        private async Task<bool> createSource()
        {
            var authorsCollection = this.sourceAuthorsListView.Items.ToList();
            var authorsObjects = authorsCollection?.Where(x =>
                x is User user && !user.Username.Equals("Click to edit") &&
                !string.IsNullOrWhiteSpace(user.Username));
            var authorsNames = authorsObjects?.Select(x => ((User)x).Username).ToList();

            var selectedDateTime = this.sourceAccessedDatePicker.SelectedDate;
            DateTime? dateTime = this.sourceAccessedDatePicker.Date.DateTime;
            if (selectedDateTime == null)
            {
                dateTime = null;
            }

            var source = new Source
            {
                Name = this.sourceNameTextBox.Text,
                Description = this.sourceDescriptionTextBox.Text,
                IsLink = this.sourceIsLinkCheckBox.IsChecked ?? true,
                AuthorsString = string.Join("|", authorsNames),
                Publisher = this.sourcePublisherTextBox.Text,
                AccessedAt = dateTime
            };

            switch (this.isPdf)
            {
                case true:
                {
                    source.Type = SourceType.Pdf.ToString();
                    break;
                }
                case false:
                {
                    source.Type = SourceType.Vid.ToString();
                    break;
                }
                default:
                {
                    await FileTypeDialog.ShowAsync();
                    return true;
                }
            }

            var isChecked = this.sourceIsLinkCheckBox.IsChecked;

            switch (isChecked)
            {
                case null:
                {
                    return true;
                }
                case false:
                    if (this.storageFile == null)
                    {
                        await FileUploadDialog.ShowAsync();
                        return true;
                    }

                    source.Link = "";
                    source.Content = await DataManager.FileToBinary(this.storageFile);
                    break;
                default:
                    if (string.IsNullOrWhiteSpace(this.sourceLinkTextBox.Text))
                    {
                        await FileLinkDialog.ShowAsync();
                        return true;
                    }

                    source.Link = this.sourceLinkTextBox.Text;
                    break;
            }

            this.NewSource = source;
            return false;
        }

        private void sourceIsLinkCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.linkOptionsGrid.Visibility = Visibility.Collapsed;
            this.uploadOptionsGrid.Visibility = Visibility.Visible;
        }

        private void sourceIsLinkCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.linkOptionsGrid.Visibility = Visibility.Visible;
            this.uploadOptionsGrid.Visibility = Visibility.Collapsed;
        }

        private async void sourceUploadButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();

            switch (this.isPdf)
            {
                case true:
                {
                    picker.FileTypeFilter.Add(".pdf");
                    break;
                }
                case false:
                {
                    picker.FileTypeFilter.Add(".mp4");
                    break;
                }
                default:
                {
                    await FileTypeDialog.ShowAsync();
                    return;
                }
            }
            var file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                this.storageFile = file;
                this.sourceUploadButton.Content = "Uploading: " + file.Name;
            }
        }

        private void AddAuthor_Click(object sender, RoutedEventArgs e)
        {
            var newAuthor = new User
            {
                Username = "Click to edit"
            };
            this.sourceAuthorsListView.Items?.Add(newAuthor);
        }

        private async void RemoveAuthor_Click(object sender, RoutedEventArgs e)
        {
            var selectedItemIndex = this.sourceAuthorsListView.SelectedIndex;
            if (selectedItemIndex != -1)
            {
                this.sourceAuthorsListView.Items?.RemoveAt(selectedItemIndex);
            }
            else
            {
                var messageDialog = new MessageDialog("Please select an author to remove.");
                await messageDialog.ShowAsync();
            }
        }

        private void sourceAuthorsListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((ListView)sender);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            var selectedItemTextBox = (User)textBox.DataContext;
            selectedItemTextBox.Username = textBox.Text;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            var selectedItem = comboBox.SelectedIndex;

            this.isPdf = selectedItem == 0;
        }

        #endregion
    }
}