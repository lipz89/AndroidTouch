using System;
using System.Drawing;
using System.Windows.Forms;

namespace AndroidHelper
{
    public partial class UcParameter : UserControl
    {
        private IParameter parameter;
        private object rawValue = null;
        private Color green = Color.Green;
        private Color red = Color.PaleVioletRed;

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
                var p = this.parameter as IParameter<int>;
                if (int.TryParse(txtValue.Text.Trim(), out var i))
                {
                    p.Value = i;
                    SetColor(green);
                }
                else
                {
                    p.Value = 0;
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
                    var p = this.parameter as IParameter<int>;
                    var i = (int)value;
                    p.Value = i;
                    this.txtValue.Text = i.ToString();
                    SetColor(green);
                    return;
                }
            }
            else if (this.parameter.Type == ParameterType.Point)
            {
                if (value is Point)
                {
                    var p = this.parameter as IParameter<Point>;
                    var point = (Point)value;
                    var valStr = $"{point.X} {point.Y}";
                    p.Value = point;
                    this.txtValue.Text = valStr;
                    SetColor(green);
                    return;
                }
            }
            else
            {
                if (value is string)
                {
                    var p = this.parameter as IParameter<string>;
                    var val = (string)value;
                    p.Value = val;
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
            this.txtValue.ForeColor = color;
        }

        public bool HasValue()
        {
            return this.parameter.Value != null;
        }
    }
}
