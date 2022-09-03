using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace GenshinModelViewer
{
    public static class StoryboardUtils
    {
        public static void BeginBrushStoryboard(DependencyObject dependencyObj, IDictionary<DependencyProperty, Brush> toDictionary, double durationSeconds = 0.2d)
        {
            var storyboard = new Storyboard();
            foreach (var keyValue in toDictionary)
            {
                var anima = new BrushAnimation()
                {
                    To = keyValue.Value,
                    Duration = TimeSpan.FromSeconds(durationSeconds),
                };
                Storyboard.SetTarget(anima, dependencyObj);
                Storyboard.SetTargetProperty(anima, new PropertyPath(keyValue.Key));
                storyboard.Children.Add(anima);
            }
            storyboard.Begin();
        }

        public static void BeginBrushStoryboard(DependencyObject dependencyObj, IList<DependencyProperty> dpList)
        {
            var storyboard = new Storyboard();
            foreach (var dp in dpList)
            {
                var anima = new BrushAnimation()
                {
                    Duration = TimeSpan.FromSeconds(0.2),
                };
                Storyboard.SetTarget(anima, dependencyObj);
                Storyboard.SetTargetProperty(anima, new PropertyPath(dp));
                storyboard.Children.Add(anima);
            }
            storyboard.Begin();
        }

        public static void BeginDoubleStoryboard(DependencyObject dependencyObj, DependencyProperty dp, double from, double to, Duration? duration = null, Action completed = null)
        {
            var storyboard = new Storyboard();
            var anima = new DoubleAnimation()
            {
                From = from,
                To = to,
                Duration = duration ?? TimeSpan.FromSeconds(0.2),
            };
            anima.EasingFunction = new ExponentialEase()
            {
                Exponent = 8,
                EasingMode = EasingMode.EaseOut,
            };
            Storyboard.SetTarget(anima, dependencyObj);
            Storyboard.SetTargetProperty(anima, new PropertyPath(dp));
            if (completed != null)
            {
                storyboard.Completed += (s, e) => completed?.Invoke();
            }
            storyboard.Children.Add(anima);
            storyboard.Begin();
        }
    }
}
