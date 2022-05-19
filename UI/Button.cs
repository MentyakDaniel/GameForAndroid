using ditto;
using ditto.mono;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;

namespace GameForAndroid.UI
{
    /// <summary>
    /// Object a regular button 
    /// </summary>
    public class Button : CCRectangle
    {
        #region Private Fields

        /// <summary>
        /// Text of the button
        /// </summary>
        private CCLabel _label;

        /// <summary>
        /// Name of <see cref="CCLabel"/> font
        /// </summary>
        private string _fontName;

        /// <summary>
        /// Button states textures
        /// </summary>
        public CCColor Hover { get; private set; } = CCColor.FromHex("#757575");
        public CCColor Pressed { get; private set; } = CCColor.FromHex("#b2ac00");
        public CCColor Idle { get; private set; } = CCColor.FromHex("#5c5c5c");

        /// <summary>
        /// The state of the mouse on the previous frame 
        /// </summary>
        private MouseState _lastMouseState;

        private int TouchId;
        #endregion

        #region Public Properties

        /// <summary>
        /// If mouse pressed the button
        /// </summary>
        public bool IsPressed { get; set; }

        /// <summary>
        /// Text of the button
        /// </summary>
        public string Text
        {
            get => _label == null ? string.Empty : _label.Text;
            set
            {
                if (_label != null)
                    _label.Text = value;
            }
        }

        /// <summary>
        /// Name of <see cref="CCLabel"/> font
        /// </summary>
        public string FontName
        {
            get => _fontName;
            set
            {
                if (value == null || value == string.Empty)
                    return;

                if (_label != null)
                    _label.Font = FontManager.GetFont(value);

                _fontName = value;
            }
        }

        /// <summary>
        /// Color of the text
        /// </summary>
        public CCColor Foreground
        {
            get => _label == null ? default : _label.Color;
            set
            {
                if (_label != null)
                    _label.Color = value;
            }
        }

        /// <summary>
        /// Size of the font
        /// </summary>
        public int FontSize
        {
            get => _label == null ? default : _label.FontSize;
            set
            {
                if (_label != null)
                    _label.FontSize = value;
            }
        }

        /// <summary>
        /// if Need draw a Background
        /// </summary>
        public bool IsDrawBackground = true;

        /// <summary>
        /// Horizontal alignment for button text
        /// </summary>
        public CCTextAlignment HorizontalAlignment
        {
            get => _label == null ? CCTextAlignment.Left : _label.HorizontalAlignment;
            set => ChangeAlignmentAndPositionText(value);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Object a regular button 
        /// </summary>
        /// <param name="text">Button text. If text null or <see cref="string.Empty"/> - <see cref="CCLabel"/> not creating</param>
        /// <param name="fontName">Name of font</param>
        /// <param name="fontSize">Size of the font</param>
        /// <param name="alignment">Horizontal alignment of text: <see cref="CCTextAlignment.Left"/>, <see cref="CCTextAlignment.Center"/> and <see cref="CCTextAlignment.Right"/></param>
        /// <param name="width">Width of the button</param>
        /// <param name="height">Height of the button</param>
        /// <param name="backgroundColor">Color of the button</param>
        public Button(string text, string fontName, int fontSize, CCTextAlignment alignment, double width, double height, CCColor backgroundColor)
            : this(text, fontName, fontSize, alignment, width, height, 0, CCColors.Transparent, backgroundColor)
        {
        }

        /// <summary>
        /// Object a regular button 
        /// </summary>
        /// <param name="text">Button text. If text null or <see cref="string.Empty"/> - <see cref="CCLabel"/> not creating</param>
        /// <param name="fontName">Name of font</param>
        /// <param name="fontSize">Size of the font</param>
        /// <param name="alignment">Horizontal alignment of text: <see cref="CCTextAlignment.Left"/>, <see cref="CCTextAlignment.Center"/> and <see cref="CCTextAlignment.Right"/></param>
        /// <param name="size">Width and Height of the button</param>
        /// <param name="backgroundColor">Color of the button</param>
        /// <param name="radius">Corner radius of the button</param>
        public Button(string text, string fontName, int fontSize, CCTextAlignment alignment, CCSize size, CCColor backgroundColor, double radius = 0)
            : this(text, fontName, fontSize, alignment, size.Width, size.Height, 0, CCColors.Transparent, backgroundColor, radius)
        {
        }

        /// <summary>
        /// Object a regular button 
        /// </summary>
        /// <param name="text">Button text. If text null or <see cref="string.Empty"/> - <see cref="CCLabel"/> not creating</param>
        /// <param name="fontName">Name of font</param>
        /// <param name="fontSize">Size of the font</param>
        /// <param name="alignment">Horizontal alignment of text: <see cref="CCTextAlignment.Left"/>, <see cref="CCTextAlignment.Center"/> and <see cref="CCTextAlignment.Right"/></param>
        /// <param name="size">Width and Height of the button</param>
        /// <param name="borderWidth">Border width of the button</param>
        /// <param name="borderColor">Border color of the button</param>
        /// <param name="backgroundColor">Color of the button</param>
        public Button(string text, string fontName, int fontSize, CCTextAlignment alignment, CCSize size, double borderWidth, CCColor borderColor, CCColor backgroundColor)
            : this(text, fontName, fontSize, alignment, size.Width, size.Height, borderWidth, borderColor, backgroundColor)
        {
        }

        /// <summary>
        /// Object a regular button 
        /// </summary>
        /// <param name="text">Button text. If text null or <see cref="string.Empty"/> - <see cref="CCLabel"/> not creating</param>
        /// <param name="fontName">Name of font</param>
        /// <param name="fontSize">Size of the font</param>
        /// <param name="alignment">Horizontal alignment of text: <see cref="CCTextAlignment.Left"/>, <see cref="CCTextAlignment.Center"/> and <see cref="CCTextAlignment.Right"/></param>
        /// <param name="size">Width and Height of the button</param>
        /// <param name="borderWidth">Border width of the button</param>
        /// <param name="borderColor">Border color of the button</param>
        /// <param name="backgroundColor">Color of the button</param>
        /// <param name="radius">Corner radius of the button</param>
        public Button(string text, string fontName, int fontSize, CCTextAlignment alignment, CCSize size, double borderWidth, CCColor borderColor, CCColor backgroundColor, double radius = 0)
            : this(text, fontName, fontSize, alignment, size.Width, size.Height, borderWidth, borderColor, backgroundColor, radius)
        {
        }

        /// <summary>
        /// Object a regular button 
        /// </summary>
        /// <param name="text">Button text. If text null or <see cref="string.Empty"/> - <see cref="CCLabel"/> not creating</param>
        /// <param name="fontName">Name of font</param>
        /// <param name="fontSize">Size of the font</param>
        /// <param name="alignment">Horizontal alignment of text: <see cref="CCTextAlignment.Left"/>, <see cref="CCTextAlignment.Center"/> and <see cref="CCTextAlignment.Right"/></param>
        /// <param name="width">Width of the button</param>
        /// <param name="height">Height of the button</param>
        /// <param name="borderWidth">Border width of the button</param>
        /// <param name="borderColor">Border color of the button</param>
        /// <param name="backgroundColor">Color of the button</param>
        /// <param name="radius">Corner radius of the button</param>
        public Button(string text, string fontName, int fontSize, CCTextAlignment alignment, double width, double height, double borderWidth, CCColor borderColor, CCColor backgroundColor, double radius = 0)
            : base(width, height, borderWidth, borderColor, backgroundColor, radius)
        {
            if (text == "" || text == null || text == string.Empty) return;
            if (fontName == "" || fontName == null || fontName == string.Empty) return;
            if (fontSize == default) return;

            Initialize(text, fontName, fontSize, alignment);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Method for change standart colors
        /// </summary>
        /// <param name="idle">Idle state</param>
        /// <param name="hover">Hover state</param>
        /// <param name="pressed">Pressed state</param>
        public void SetColors(CCColor idle, CCColor hover, CCColor pressed)
        {
            Idle = idle;
            Hover = hover;
            Pressed = pressed;
        }
        public void Update(GameTime gameTime, TouchCollection touches)
        {
            if (!Visible) return;

            foreach (var i in touches)
            {
                if (RenderRectangle.Contains(i.Position) && TouchId != i.Id)
                {
                    IsPressed = true;
                    TouchId = i.Id;
                }
                else
                    IsPressed = false;
            }
            if (touches.Count == 0)
                IsPressed = false;

            base.Update(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (!Visible) return;

            MouseState state = Mouse.GetState();

            Vector2 mousePos = new Vector2(state.X, state.Y);

            if (Camera != null)
                mousePos = Camera.GetMousePosition(mousePos.X, mousePos.Y);

            if (RenderRectangle.Contains(mousePos))
            {
                switch (state.LeftButton)
                {
                    case ButtonState.Pressed:
                        Background = Pressed;
                        break;
                    default:
                        if (_lastMouseState.LeftButton == ButtonState.Pressed)
                            IsPressed = true;
                        else
                        {
                            Background = Hover;
                            IsPressed = false;
                        }
                        break;
                }
            }
            else
            {
                Background = Idle;
                IsPressed = false;
            }

            _lastMouseState = state;

            base.Update(gameTime);
        }

        public override void Draw(GraphicsDevice device)
        {
            if (IsDrawBackground)
                base.Draw(device);
            else
                foreach (CCNode i in Children)
                    i.Draw(device);
        }

        /// <summary>
        /// Method for add Text to the button
        /// </summary>
        /// <param name="text"></param>
        /// <param name="fontName"></param>
        /// <param name="fontSize"></param>
        /// <param name="alignment"></param>
        public void SetText(string text, string fontName, int fontSize, CCTextAlignment alignment)
        {
            if (_label != null) { RemoveChild(_label); _label = null; }

            _label = new CCLabel(text, fontName, fontSize, alignment);

            AddChild(_label);
        }

        #endregion

        #region Private Methods

        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (propertyName == nameof(Position))
                ChangeAlignmentAndPositionText(HorizontalAlignment);

            base.OnPropertyChanged(propertyName);
        }
        /// <summary>
        /// Method for initial object configuration 
        /// </summary>
        /// <param name="text">Button text</param>
        /// <param name="fontName">Text font name</param>
        /// <param name="fontSize">Text font size</param>
        /// <param name="alignment">Horizontal alignment</param>
        private void Initialize(string text, string fontName, int fontSize, CCTextAlignment alignment)
        {
            _label = new CCLabel(text, fontName, fontSize, alignment);
            HorizontalAlignment = alignment;
            AddChild(_label);
        }

        /// <summary>
        /// Method for change text alignment
        /// </summary>
        /// <param name="value">New Alignment value</param>
        private void ChangeAlignmentAndPositionText(CCTextAlignment value)
        {
            if (_label != null)
            {
                _label.HorizontalAlignment = value;
                _label.Position = HorizontalAlignment switch
                {

                    CCTextAlignment.Left => new CCPoint(RenderRectangle.X + 10, RenderRectangle.Center.Y - _label.Height / 2),
                    CCTextAlignment.Center => new CCPoint(RenderRectangle.Center.X - _label.Width / 2, RenderRectangle.Center.Y - _label.Height / 2),
                    CCTextAlignment.Right => new CCPoint(RenderRectangle.Right - 10 - _label.Width, RenderRectangle.Center.Y - _label.Height / 2),
                    _ => throw new NotImplementedException(),
                };
            }
        }

        #endregion
    }
}