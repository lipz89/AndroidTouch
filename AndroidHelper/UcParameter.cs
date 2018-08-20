using System;
using System.Drawing;
using System.Windows.Forms;

namespace AndroidHelper
{
    public partial class UcParameter : UserControl
    {
        private IParameter parameter;
        private object rawValue = null;
        private object newValue = null;
        private Color green = Color.LightGreen;
        private Color red = Color.HotPink;

        public UcParameter()
        {
            InitializeComponent();
            this.txtValue.GotFocus += TxtValue_GotFocus;
            this.lblInfo.Click += TxtValue_GotFocus;
            this.Click += TxtValue_GotFocus;
            SetColor(red);
        }

        private void TxtValue_GotFocus(object sender, EventArgs e)
        {
            this.OnGotFocus(EventArgs.Empty);
        }

        internal ParameterType ParameterType { get; set; }

        internal IParameter Parameter
        {
            get => parameter;
            set
            {
                parameter = value;
                if (parameter != null)
                {
                    ParameterType = parameter.Type;
                    rawValue = parameter.Value;
                    this.lblInfo.Text = $"{parameter.Name},名称{parameter.Display},类型{parameter.Type}";
                    if (parameter.Type == ParameterType.Point)
                    {
                        this.txtValue.ReadOnly = true;
                    }
                    else
                    {
                        this.txtValue.TextChanged += TxtValue_TextChanged;
                    }

                    if (parameter.Value != null)
                    {
                        this.txtValue.Text = parameter.Value.ToString();
                        SetColor(green);
                    }
                }
            }
        }

        private void TxtValue_TextChanged(object sender, EventArgs e)
        {
            if (this.parameter.Type == ParameterType.Int)
            {
                if (int.TryParse(txtValue.Text.Trim(), out var i))
                {
                    newValue = i;
                    SetColor(green);
                }
                else
                {
                    SetColor(Color.PaleVioletRed);
                }
            }
        }

        public void GetUserFocus()
        {
            this.BackColor = SystemColors.Highlight;
            this.lblInfo.BackColor = SystemColors.Highlight;
            this.lblInfo.ForeColor = SystemColors.HighlightText;
        }
        public void LostUserFocus()
        {
            this.BackColor = SystemColors.Control;
            this.lblInfo.BackColor = SystemColors.Control;
            this.lblInfo.ForeColor = SystemColors.InfoText;
        }

        public void SetValue(object value)
        {
            if (this.parameter.Type == ParameterType.Int)
            {
                if (value is int)
                {
                    var i = (int)value;
                    newValue = i;
                    this.txtValue.Text = i.ToString();
                    SetColor(green);
                    return;
                }
            }
            else if (this.parameter.Type == ParameterType.Point)
            {
                if (value is Point)
                {
                    var point = (Point)value;
                    var valStr = $"{point.X} {point.Y}";
                    newValue = point;
                    this.txtValue.Text = valStr;
                    SetColor(green);
                    return;
                }
            }
            else
            {
                if (value is string)
                {
                    var val = (string)value;
                    newValue = val;
                    this.txtValue.Text = val;
                    SetColor(green);
                    return;
                }
            }
            SetColor(Color.PaleVioletRed);
        }

        public void Reset()
        {
            this.txtValue.Text = rawValue.ToString();
            this.parameter.Value = rawValue;
            if (this.HasValue())
            {
                SetColor(green);
            }
            else
            {
                SetColor(Color.PaleVioletRed);
            }
        }

        private void SetColor(Color color)
        {
            this.txtValue.BackColor = color;
        }

        public bool HasValue()
        {
            return this.parameter.Value != null;
        }

        public void Apply()
        {
            if (this.parameter.Type == ParameterType.Int)
            {
                if (newValue is int i && parameter is Parameter<int> p)
                {
                    p.Value = i;
                }
            }
            else if (this.parameter.Type == ParameterType.Point)
            {
                if (newValue is Point i && parameter is Parameter<Point> p)
                {
                    p.Value = i;
                }
            }
            else
            {
                if (newValue is string i && parameter is Parameter<string> p)
                {
                    p.Value = i;
                }
            }
        }
    }
}
