﻿//-----------------------------------------------------------------------
// <copyright file="ModernDataGridView.cs" company="YuGuan Corporation">
//     Copyright (c) YuGuan Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DevLib.ModernUI.Forms
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using DevLib.ModernUI.ComponentModel;
    using DevLib.ModernUI.Drawing;

    /// <summary>
    /// ModernDataGridView user control.
    /// </summary>
    [ToolboxBitmap(typeof(DataGridView))]
    public class ModernDataGridView : DataGridView, IModernControl
    {
        /// <summary>
        /// Field _modernColorStyle.
        /// </summary>
        private ModernColorStyle _modernColorStyle = ModernColorStyle.Default;

        /// <summary>
        /// Field _modernThemeStyle.
        /// </summary>
        private ModernThemeStyle _modernThemeStyle = ModernThemeStyle.Default;

        /// <summary>
        /// Field _styleManager.
        /// </summary>
        private ModernStyleManager _styleManager = null;

        /// <summary>
        /// Field _components.
        /// </summary>
        private IContainer _components = null;

        /// <summary>
        /// Field _verticalScrollBar.
        /// </summary>
        private ModernScrollBar _verticalScrollBar = new ModernScrollBar();

        /// <summary>
        /// Field _horizontalScrollBar.
        /// </summary>
        private ModernScrollBar _horizontalScrollBar = new ModernScrollBar();

        /// <summary>
        /// Field _verticalScrollBarHelper.
        /// </summary>
        private ModernDataGridViewHelper _verticalScrollBarHelper = null;

        /// <summary>
        /// Field _horizontalScrollBarHelper.
        /// </summary>
        private ModernDataGridViewHelper _horizontalScrollBarHelper = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModernDataGridView"/> class.
        /// </summary>
        public ModernDataGridView()
        {
            this.InitializeComponent();

            this.HighlightPercentage = 0.2f;

            this.Controls.Add(this._verticalScrollBar);
            this.Controls.Add(this._horizontalScrollBar);

            this.Controls.SetChildIndex(this._verticalScrollBar, 0);
            this.Controls.SetChildIndex(this._horizontalScrollBar, 1);

            this._verticalScrollBar.Visible = false;
            this._horizontalScrollBar.Visible = false;

            this._verticalScrollBarHelper = new ModernDataGridViewHelper(this._verticalScrollBar, this, true);
            this._horizontalScrollBarHelper = new ModernDataGridViewHelper(this._horizontalScrollBar, this, false);

            this.HorizontalScrollBarSize = 20;
            this.VerticalScrollBarSize = 20;

            this.HorizontalScrollBar.MaximumSize = new Size(1, 0);
            this.VerticalScrollBar.MaximumSize = new Size(0, 1);

            this.ApplyModernStyle();
        }

        /// <summary>
        /// Event CustomPaintBackground.
        /// </summary>
        [Category("Modern Appearance")]
        public event EventHandler<ModernPaintEventArgs> CustomPaintBackground;

        /// <summary>
        /// Event CustomPaint.
        /// </summary>
        [Category("Modern Appearance")]
        public event EventHandler<ModernPaintEventArgs> CustomPaint;

        /// <summary>
        /// Event CustomPaintForeground.
        /// </summary>
        [Category("Modern Appearance")]
        public event EventHandler<ModernPaintEventArgs> CustomPaintForeground;

        /// <summary>
        /// Gets or sets modern color style.
        /// </summary>
        [Category("Modern Appearance")]
        [DefaultValue(ModernColorStyle.Default)]
        public ModernColorStyle ColorStyle
        {
            get
            {
                if (this.DesignMode || this._modernColorStyle != ModernColorStyle.Default)
                {
                    return this._modernColorStyle;
                }

                if (this.StyleManager != null && this._modernColorStyle == ModernColorStyle.Default)
                {
                    return this.StyleManager.ColorStyle;
                }

                if (this.StyleManager == null && this._modernColorStyle == ModernColorStyle.Default)
                {
                    return ModernColorStyle.Blue;
                }

                return this._modernColorStyle;
            }

            set
            {
                this._modernColorStyle = value;
                this.ApplyModernStyle();
                this.RefreshScrollBarHelper();
            }
        }

        /// <summary>
        /// Gets or sets modern theme style.
        /// </summary>
        [Category("Modern Appearance")]
        [DefaultValue(ModernThemeStyle.Default)]
        public ModernThemeStyle ThemeStyle
        {
            get
            {
                if (this.DesignMode || this._modernThemeStyle != ModernThemeStyle.Default)
                {
                    return this._modernThemeStyle;
                }

                if (this.StyleManager != null && this._modernThemeStyle == ModernThemeStyle.Default)
                {
                    return this.StyleManager.ThemeStyle;
                }

                if (this.StyleManager == null && this._modernThemeStyle == ModernThemeStyle.Default)
                {
                    return ModernThemeStyle.Light;
                }

                return this._modernThemeStyle;
            }

            set
            {
                this._modernThemeStyle = value;
                this.ApplyModernStyle();
                this.RefreshScrollBarHelper();
            }
        }

        /// <summary>
        /// Gets or sets modern style manager.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ModernStyleManager StyleManager
        {
            get
            {
                return this._styleManager;
            }

            set
            {
                this._styleManager = value;
                this.ApplyModernStyle();
                this.RefreshScrollBarHelper();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether use custom BackColor.
        /// </summary>
        [DefaultValue(false)]
        [Category("Modern Appearance")]
        public bool UseCustomBackColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether use custom ForeColor.
        /// </summary>
        [DefaultValue(false)]
        [Category("Modern Appearance")]
        public bool UseCustomForeColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether use StyleColors.
        /// </summary>
        [DefaultValue(false)]
        [Category("Modern Appearance")]
        public bool UseStyleColors
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control can receive focus.
        /// </summary>
        [Browsable(false)]
        [Category("Modern Behaviour")]
        [DefaultValue(true)]
        public bool UseSelectable
        {
            get
            {
                return this.GetStyle(ControlStyles.Selectable);
            }

            set
            {
                this.SetStyle(ControlStyles.Selectable, value);
            }
        }

        /// <summary>
        /// Gets or sets the highlight percentage.
        /// </summary>
        [Category("Modern Behaviour")]
        [DefaultValue(0.2F)]
        public float HighlightPercentage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the size of the vertical scroll bar.
        /// </summary>
        [Category("Modern Behaviour")]
        [DefaultValue(20)]
        public int VerticalScrollBarSize
        {
            get
            {
                return this._verticalScrollBar.ScrollbarSize;
            }

            set
            {
                this._verticalScrollBar.ScrollbarSize = value;
                this._horizontalScrollBarHelper.CornerSize = value;
                this.RefreshScrollBarHelper();
            }
        }

        /// <summary>
        /// Gets or sets the size of the horizontal scroll bar.
        /// </summary>
        [Category("Modern Behaviour")]
        [DefaultValue(20)]
        public int HorizontalScrollBarSize
        {
            get
            {
                return this._horizontalScrollBar.ScrollbarSize;
            }

            set
            {
                this._horizontalScrollBar.ScrollbarSize = value;
                this._verticalScrollBarHelper.CornerSize = value;
                this.RefreshScrollBarHelper();
            }
        }

        /// <summary>
        /// Forces the control to invalidate its client area and immediately redraw itself and any child controls.
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();

            this.RefreshScrollBarHelper();
        }

        /// <summary>
        /// Raises the <see cref="E:CustomPaintBackground" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ModernPaintEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCustomPaintBackground(ModernPaintEventArgs e)
        {
            if (this.GetStyle(ControlStyles.UserPaint) && this.CustomPaintBackground != null)
            {
                this.CustomPaintBackground(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:CustomPaint" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ModernPaintEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCustomPaint(ModernPaintEventArgs e)
        {
            if (this.GetStyle(ControlStyles.UserPaint) && this.CustomPaint != null)
            {
                this.CustomPaint(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:CustomPaintForeground" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ModernPaintEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCustomPaintForeground(ModernPaintEventArgs e)
        {
            if (this.GetStyle(ControlStyles.UserPaint) && this.CustomPaintForeground != null)
            {
                this.CustomPaintForeground(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnHandleCreated(EventArgs e)
        {
            this.RefreshScrollBarHelper();

            base.OnHandleCreated(e);
        }

        /// <summary>
        /// Raises the GotFocus event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnGotFocus(EventArgs e)
        {
            this.RefreshScrollBarHelper();

            base.OnGotFocus(e);
        }

        /// <summary>
        /// Raises the VisibleChanged event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnVisibleChanged(EventArgs e)
        {
            this.RefreshScrollBarHelper();

            base.OnVisibleChanged(e);
        }

        /// <summary>
        /// OnMouseWheel method.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (e.Delta > 0 && this.FirstDisplayedScrollingRowIndex > 0)
            {
                try
                {
                    this.FirstDisplayedScrollingRowIndex--;
                }
                catch
                {
                }
            }
            else if (e.Delta < 0)
            {
                try
                {
                    this.FirstDisplayedScrollingRowIndex++;
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:Paint" /> event.
        /// </summary>
        /// <param name="e">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (this.HorizontalScrollBar.Visible || this.VerticalScrollBar.Visible)
            {
                Color backColor = this.BackColor;

                if (!this.UseCustomBackColor)
                {
                    backColor = ModernPaint.BackColor.Form(this.ThemeStyle);
                }

                using (SolidBrush brush = new SolidBrush(backColor))
                {
                    if (this.HorizontalScrollBar.Visible)
                    {
                        e.Graphics.FillRectangle(brush, 0, this.Height - this.HorizontalScrollBar.Height, this.Width, this.HorizontalScrollBar.Height);
                    }

                    if (this.VerticalScrollBar.Visible)
                    {
                        e.Graphics.FillRectangle(brush, this.Width - this.VerticalScrollBar.Width, 0, this.VerticalScrollBar.Width, this.Height);
                    }
                }
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this._components != null))
            {
                this._components.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Called to update scroll bar.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnUpdateScrollBar(object sender, EventArgs e)
        {
            this.RefreshScrollBarHelper();
        }

        /// <summary>
        /// Required method for Designer support - do not modify the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ((ISupportInitialize)this).BeginInit();
            this.SuspendLayout();

            this._horizontalScrollBar.LargeChange = 10;
            this._horizontalScrollBar.Location = new Point(0, 0);
            this._horizontalScrollBar.Maximum = 100;
            this._horizontalScrollBar.Minimum = 0;
            this._horizontalScrollBar.MouseWheelBarPartitions = 10;
            this._horizontalScrollBar.Name = "_horizontalScrollBar";
            this._horizontalScrollBar.Orientation = ModernScrollBarOrientation.Horizontal;
            this._horizontalScrollBar.ScrollbarSize = 50;
            this._horizontalScrollBar.TabIndex = 0;
            this._horizontalScrollBar.UseSelectable = true;

            this._verticalScrollBar.LargeChange = 10;
            this._verticalScrollBar.Location = new Point(0, 0);
            this._verticalScrollBar.Maximum = 100;
            this._verticalScrollBar.Minimum = 0;
            this._verticalScrollBar.MouseWheelBarPartitions = 10;
            this._verticalScrollBar.Name = "_verticalScrollBar";
            this._verticalScrollBar.Orientation = ModernScrollBarOrientation.Vertical;
            this._verticalScrollBar.ScrollbarSize = 50;
            this._verticalScrollBar.TabIndex = 0;
            this._verticalScrollBar.UseSelectable = true;

            ((ISupportInitialize)this).EndInit();
            this.ResumeLayout(false);

            this.RowsAdded += this.OnUpdateScrollBar;
            this.UserDeletedRow += this.OnUpdateScrollBar;
            this.RowHeightChanged += this.OnUpdateScrollBar;
            this.ColumnAdded += this.OnUpdateScrollBar;
            this.ColumnRemoved += this.OnUpdateScrollBar;
            this.ColumnWidthChanged += this.OnUpdateScrollBar;
            this.Scroll += this.OnUpdateScrollBar;
            this.Resize += this.OnUpdateScrollBar;
            this.HorizontalScrollBar.VisibleChanged += this.OnUpdateScrollBar;
            this.VerticalScrollBar.VisibleChanged += this.OnUpdateScrollBar;
            this.Layout += this.OnUpdateScrollBar;
        }

        /// <summary>
        /// Apply modern style.
        /// </summary>
        private void ApplyModernStyle()
        {
            this.BorderStyle = BorderStyle.None;
            this.CellBorderStyle = DataGridViewCellBorderStyle.None;
            this.EnableHeadersVisualStyles = false;
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.Font = new Font("Segoe UI", 11f, FontStyle.Regular, GraphicsUnit.Pixel);
            this.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.AllowUserToResizeRows = false;
            this.AllowUserToResizeColumns = true;
            this.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            this.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            if (!this.UseCustomBackColor)
            {
                this.BackColor = ModernPaint.BackColor.Form(this.ThemeStyle);
                this.BackgroundColor = ModernPaint.BackColor.Form(this.ThemeStyle);
                this.GridColor = ModernPaint.BackColor.Form(this.ThemeStyle);
                this.ColumnHeadersDefaultCellStyle.BackColor = ModernPaint.GetStyleColor(this.ColorStyle);
                this.RowHeadersDefaultCellStyle.BackColor = ModernPaint.GetStyleColor(this.ColorStyle);
                this.DefaultCellStyle.BackColor = ModernPaint.BackColor.Form(this.ThemeStyle);
                this.DefaultCellStyle.SelectionBackColor = ControlPaint.Light(ModernPaint.GetStyleColor(this.ColorStyle), this.HighlightPercentage);
                this.RowsDefaultCellStyle.BackColor = ModernPaint.BackColor.Form(this.ThemeStyle);
                this.RowsDefaultCellStyle.SelectionBackColor = ControlPaint.Light(ModernPaint.GetStyleColor(this.ColorStyle), this.HighlightPercentage);
                this.RowHeadersDefaultCellStyle.SelectionBackColor = ControlPaint.Light(ModernPaint.GetStyleColor(this.ColorStyle), this.HighlightPercentage);
                this.ColumnHeadersDefaultCellStyle.SelectionBackColor = ControlPaint.Light(ModernPaint.GetStyleColor(this.ColorStyle), this.HighlightPercentage);
            }

            if (!this.UseCustomForeColor)
            {
                this.ForeColor = ModernPaint.ForeColor.Button.Disabled(this.ThemeStyle);
                this.ColumnHeadersDefaultCellStyle.ForeColor = ModernPaint.ForeColor.Button.Press(this.ThemeStyle);
                this.RowHeadersDefaultCellStyle.ForeColor = ModernPaint.ForeColor.Button.Press(this.ThemeStyle);
                this.DefaultCellStyle.SelectionForeColor = Color.FromArgb(17, 17, 17);
                this.RowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(17, 17, 17);
                this.RowHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(17, 17, 17);
                this.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(17, 17, 17);
            }
        }

        /// <summary>
        /// Refreshes the scroll bar helper.
        /// </summary>
        private void RefreshScrollBarHelper()
        {
            if (this._verticalScrollBarHelper != null)
            {
                this._verticalScrollBarHelper.UpdateScrollBar();
            }

            if (this._horizontalScrollBarHelper != null)
            {
                this._horizontalScrollBarHelper.UpdateScrollBar();
            }
        }
    }
}
