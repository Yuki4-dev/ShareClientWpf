using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ShareClientWpf
{
    /// <summary>
    /// ComboBoxEX.xaml の相互作用ロジック
    /// </summary>
    public partial class CustomComboBox : ComboBox
    {
        public CustomComboBoxSelectVisual SelectVisual
        {
            get => (CustomComboBoxSelectVisual)GetValue(SelectVisualProperty);
            set => SetValue(SelectVisualProperty, value);
        }
        public static readonly DependencyProperty SelectVisualProperty =
            DependencyProperty.Register(nameof(SelectVisual), typeof(CustomComboBoxSelectVisual), typeof(CustomComboBox), new PropertyMetadata(CustomComboBoxSelectVisual.Overlay));

        public Brush MouseOverBrush
        {
            get => (Brush)GetValue(MouseOverBrushProperty);
            set => SetValue(MouseOverBrushProperty, value);
        }
        public static readonly DependencyProperty MouseOverBrushProperty =
            DependencyProperty.Register(nameof(MouseOverBrush), typeof(Brush), typeof(CustomComboBox), new PropertyMetadata(new SolidColorBrush(Colors.LightGray)));

        public Brush IsCheckedBrush
        {
            get => (Brush)GetValue(IsCheckedBrushProperty);
            set => SetValue(IsCheckedBrushProperty, value);
        }
        public static readonly DependencyProperty IsCheckedBrushProperty =
            DependencyProperty.Register(nameof(IsCheckedBrush), typeof(Brush), typeof(CustomComboBox), new PropertyMetadata(new SolidColorBrush(Colors.DarkGray)));

        public Brush PopupBackground
        {
            get => (Brush)GetValue(PopupBackgroundProperty);
            set => SetValue(PopupBackgroundProperty, value);
        }
        public static readonly DependencyProperty PopupBackgroundProperty =
            DependencyProperty.Register(nameof(PopupBackground), typeof(Brush), typeof(CustomComboBox), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        public Brush PopupScrollBarBrush
        {
            get => (Brush)GetValue(PopupScrollBarBrushProperty);
            set => SetValue(PopupScrollBarBrushProperty, value);
        }
        public static readonly DependencyProperty PopupScrollBarBrushProperty =
            DependencyProperty.Register(nameof(PopupScrollBarBrush), typeof(Brush), typeof(CustomComboBox), new PropertyMetadata(new SolidColorBrush(Colors.Gray)));

        public Brush SelectedItemBrush
        {
            get => (Brush)GetValue(SelectedItemBrushProperty);
            set => SetValue(SelectedItemBrushProperty, value);
        }
        public static readonly DependencyProperty SelectedItemBrushProperty =
            DependencyProperty.Register(nameof(SelectedItemBrush), typeof(Brush), typeof(CustomComboBox), new PropertyMetadata(new SolidColorBrush(Colors.DarkGray)));

        public Brush MouseOverItemBrush
        {
            get => (Brush)GetValue(MouseOverItemBrushProperty);
            set => SetValue(MouseOverItemBrushProperty, value);
        }
        public static readonly DependencyProperty MouseOverItemBrushProperty =
            DependencyProperty.Register(nameof(MouseOverItemBrush), typeof(Brush), typeof(CustomComboBox), new PropertyMetadata(new SolidColorBrush(Colors.LightGray)));

        public Brush PopupBorderBrush
        {
            get => (Brush)GetValue(PopupBorderBrushProperty);
            set => SetValue(PopupBorderBrushProperty, value);
        }
        public static readonly DependencyProperty PopupBorderBrushProperty =
            DependencyProperty.Register(nameof(PopupBorderBrush), typeof(Brush), typeof(CustomComboBox), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public Thickness PopupBorderThickness
        {
            get => (Thickness)GetValue(PopupBorderThicknessProperty);
            set => SetValue(PopupBorderThicknessProperty, value);
        }
        public static readonly DependencyProperty PopupBorderThicknessProperty =
            DependencyProperty.Register(nameof(PopupBorderThickness), typeof(Thickness), typeof(CustomComboBox), new PropertyMetadata(new Thickness(1)));

        public Thickness ItemMargin
        {
            get => (Thickness)GetValue(ItemMarginProperty);
            set => SetValue(ItemMarginProperty, value);
        }
        public static readonly DependencyProperty ItemMarginProperty =
            DependencyProperty.Register(nameof(ItemMargin), typeof(Thickness), typeof(CustomComboBox), new PropertyMetadata(new Thickness(0)));

        public CornerRadius ButtonCornerRadius
        {
            get => (CornerRadius)GetValue(ButtonCornerRadiusProperty);
            set => SetValue(ButtonCornerRadiusProperty, value);
        }
        public static readonly DependencyProperty ButtonCornerRadiusProperty =
            DependencyProperty.Register(nameof(ButtonCornerRadius), typeof(CornerRadius), typeof(CustomComboBox), new PropertyMetadata(new CornerRadius(0)));

        public CornerRadius PopupCornerRadius
        {
            get => (CornerRadius)GetValue(PopupCornerRadiusProperty);
            set => SetValue(PopupCornerRadiusProperty, value);
        }
        public static readonly DependencyProperty PopupCornerRadiusProperty =
            DependencyProperty.Register(nameof(PopupCornerRadius), typeof(CornerRadius), typeof(CustomComboBox), new PropertyMetadata(new CornerRadius(0)));

        public CornerRadius ItemSelectCornerRadius
        {
            get => (CornerRadius)GetValue(ItemSelectCornerRadiusProperty);
            set => SetValue(ItemSelectCornerRadiusProperty, value);
        }
        public static readonly DependencyProperty ItemSelectCornerRadiusProperty =
            DependencyProperty.Register(nameof(ItemSelectCornerRadius), typeof(CornerRadius), typeof(CustomComboBox), new PropertyMetadata(new CornerRadius(0)));

        public new bool IsEditable
        {
            get => false;
            set
            {
                if (value)
                {
                    throw new NotSupportedException("IsEditable True Value.");
                }
            }
        }

        public CustomComboBox()
        {
            InitializeComponent();
        }
    }

    public enum CustomComboBoxSelectVisual
    {
        Overlay, Tab
    }
}
