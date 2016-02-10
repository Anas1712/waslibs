﻿using System;

using Windows.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace AppStudio.Uwp.Controls
{
    public class PivoramaTabs : PivoramaPanel
    {
        public double PrevTabWidth { get; private set; }
        public double SelectedTabWidth { get; private set; }

        protected override int MaxItems
        {
            get { return 24; }
        }

        #region SelectedItemTemplate
        public DataTemplate SelectedItemTemplate
        {
            get { return (DataTemplate)GetValue(SelectedItemTemplateProperty); }
            set { SetValue(SelectedItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemTemplateProperty = DependencyProperty.Register("SelectedItemTemplate", typeof(DataTemplate), typeof(PivoramaTabs), new PropertyMetadata(null));
        #endregion

        protected override Size MeasureOverride(Size availableSize)
        {
            int inx = this.Index;
            int count = _items.Count;

            double x = 0;
            double maxHeight = 0;

            if (count > 0)
            {
                for (int n = 0; n < MaxItems; n++)
                {
                    var pane = this.Children[(inx + n).Mod(MaxItems)] as ContentControl;
                    if (n == 1)
                    {
                        pane.ContentTemplate = ItemTemplate;
                    }
                    else
                    {
                        pane.ContentTemplate = SelectedItemTemplate;
                    }

                    if (n <= count)
                    {
                        pane.Content = _items[(inx + n - 1).Mod(count)];
                    }
                    else
                    {
                        pane.Content = null;
                    }
                    pane.Measure(availableSize);
                    maxHeight = Math.Max(maxHeight, pane.DesiredSize.Height);
                    x += pane.DesiredSize.Width;

                    if (n == 0)
                    {
                        PrevTabWidth = pane.DesiredSize.Width;
                    }
                    if (n == 1)
                    {
                        SelectedTabWidth = pane.DesiredSize.Width;
                    }
                }
            }

            return new Size(x, maxHeight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            int inx = this.Index;
            int count = _items.Count;

            double x = 0;

            if (count > 0)
            {
                for (int n = 0; n < MaxItems; n++)
                {
                    var pane = this.Children[(inx + n).Mod(MaxItems)] as ContentControl;
                    if (n == 0)
                    {
                        x = -pane.DesiredSize.Width;
                    }

                    pane.Arrange(new Rect(x, 0, pane.DesiredSize.Width, finalSize.Height));
                    x += pane.DesiredSize.Width;
                }
            }

            return new Size(0, finalSize.Height);
        }
    }
}
