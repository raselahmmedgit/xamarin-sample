using Covi.Features.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Covi.Features.Components.DialogPage
{
    public abstract class DialogPage : OverlayPage
    {
        protected const uint DefaultDuration = 250;

        private double _defaultTranslationX;
        private double _defaultTranslationY;

        private readonly List<Task> _taskList = new List<Task>();
        private IDisposable _onDataLoadedInteractionDisposable;
        private IDisposable _onCloseInteractionDisposable;

        public uint DurationIn { get; set; } = DefaultDuration;

        public uint DurationOut { get; set; } = DefaultDuration;

        public Easing EasingIn { get; set; } = Easing.CubicInOut;

        public Easing EasingOut { get; set; } = Easing.CubicInOut;

        public abstract View OverlayView { get; }
        public abstract View DialogContentView { get; }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is DialogViewModelBase vm)
            {
                _onDataLoadedInteractionDisposable = vm.OnDialogDataLoadedInteraction.RegisterHandler(HandleOnDataLoadedAsync);
                _onCloseInteractionDisposable = vm.OnDialogCloseInteraction.RegisterHandler(HandleOnCloseAsync);
            }

            await SetupAppearingMoveAnimation(DialogContentView, OverlayView, MoveAnimationOptions.Bottom);
            await StartAnimation();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            _onDataLoadedInteractionDisposable?.Dispose();
            _onCloseInteractionDisposable?.Dispose();
        }

        private async Task HandleOnDataLoadedAsync(InteractionContext<Unit, Unit> arg)
        {
            await StartAnimation();
            arg.SetOutput(Unit.Default);
        }

        private async Task HandleOnCloseAsync(InteractionContext<Unit, Unit> arg)
        {
            await SetupDisappearingMoveAnimation(DialogContentView, OverlayView, MoveAnimationOptions.Bottom);
            await StartAnimation();
            arg.SetOutput(Unit.Default);
        }

        public async Task SetupAppearingMoveAnimation(View content, View overlayView, MoveAnimationOptions positionIn)
        {
            _taskList.Clear();

            Opacity = 0f;
            overlayView.Opacity = 0f;

            await Task.Delay(100);

            if (content != null)
            {
                UpdateDefaultTranslations(content);

                var topOffset = GetTopOffset(content, this);
                var leftOffset = GetLeftOffset(content, this);

                if (positionIn == MoveAnimationOptions.Top)
                {
                    content.TranslationY = -topOffset;
                }
                else if (positionIn == MoveAnimationOptions.Bottom)
                {
                    content.TranslationY = topOffset;
                }
                else if (positionIn == MoveAnimationOptions.Left)
                {
                    content.TranslationX = -leftOffset;
                }
                else if (positionIn == MoveAnimationOptions.Right)
                {
                    content.TranslationX = leftOffset;
                }

                _taskList.Add(content.TranslateTo(_defaultTranslationX, _defaultTranslationY, DurationIn, EasingIn));
                _taskList.Add(overlayView.FadeTo(1, DurationIn, EasingIn));
                _taskList.Add(this.FadeTo(1, DurationIn, EasingIn));
            }
        }

        public Task SetupDisappearingMoveAnimation(View content, View overlayView, MoveAnimationOptions positionOut)
        {
            _taskList.Clear();

            if (content != null)
            {
                UpdateDefaultTranslations(content);

                var topOffset = GetTopOffset(content, this);
                var leftOffset = GetLeftOffset(content, this);

                if (positionOut == MoveAnimationOptions.Top)
                {
                    _taskList.Add(content.TranslateTo(_defaultTranslationX, -topOffset, DurationOut, EasingOut));
                }
                else if (positionOut == MoveAnimationOptions.Bottom)
                {
                    _taskList.Add(content.TranslateTo(_defaultTranslationX, topOffset, DurationOut, EasingOut));
                }
                else if (positionOut == MoveAnimationOptions.Left)
                {
                    _taskList.Add(content.TranslateTo(-leftOffset, _defaultTranslationY, DurationOut, EasingOut));
                }
                else if (positionOut == MoveAnimationOptions.Right)
                {
                    _taskList.Add(content.TranslateTo(leftOffset, _defaultTranslationY, DurationOut, EasingOut));
                }
            }

            _taskList.Add(overlayView.FadeTo(0, DurationOut, EasingOut));

            return Task.FromResult(true);
        }

        private async Task StartAnimation()
        {
            await Task.WhenAll(_taskList);
        }

        private int GetTopOffset(View content, Page page)
        {
            return (int)(content.Height + page.Height) / 2;
        }

        private int GetLeftOffset(View content, Page page)
        {
            return (int)(content.Width + page.Width) / 2;
        }

        private void UpdateDefaultTranslations(View content)
        {
            _defaultTranslationX = content.TranslationX;
            _defaultTranslationY = content.TranslationY;
        }

        public enum MoveAnimationOptions
        {
            Center,
            Left,
            Right,
            Top,
            Bottom
        }
    }
}
