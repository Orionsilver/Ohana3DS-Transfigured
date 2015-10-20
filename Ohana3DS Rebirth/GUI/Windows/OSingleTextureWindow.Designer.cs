﻿namespace Ohana3DS_Rebirth.GUI
{
    partial class OSingleTextureWindow
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">verdade se for necessário descartar os recursos gerenciados; caso contrário, falso.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region código gerado pelo Component Designer

        /// <summary>
        /// Método necessário para suporte do Designer - não modifique
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OSingleTextureWindow));
            this.TopControls = new System.Windows.Forms.TableLayoutPanel();
            this.BtnExport = new Ohana3DS_Rebirth.GUI.OButton();
            this.BtnImport = new Ohana3DS_Rebirth.GUI.OButton();
            this.TexturePreview = new System.Windows.Forms.PictureBox();
            this.TopControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TexturePreview)).BeginInit();
            this.SuspendLayout();
            // 
            // TopControls
            // 
            this.TopControls.ColumnCount = 2;
            this.TopControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.TopControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.TopControls.Controls.Add(this.BtnExport, 0, 0);
            this.TopControls.Controls.Add(this.BtnImport, 0, 0);
            this.TopControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopControls.Location = new System.Drawing.Point(0, 16);
            this.TopControls.Name = "TopControls";
            this.TopControls.RowCount = 1;
            this.TopControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TopControls.Size = new System.Drawing.Size(512, 24);
            this.TopControls.TabIndex = 6;
            // 
            // BtnExport
            // 
            this.BtnExport.BackColor = System.Drawing.Color.Transparent;
            this.BtnExport.Centered = true;
            this.BtnExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnExport.Hover = true;
            this.BtnExport.Image = ((System.Drawing.Bitmap)(resources.GetObject("BtnExport.Image")));
            this.BtnExport.Location = new System.Drawing.Point(256, 0);
            this.BtnExport.Margin = new System.Windows.Forms.Padding(0);
            this.BtnExport.Name = "BtnExport";
            this.BtnExport.Size = new System.Drawing.Size(256, 24);
            this.BtnExport.TabIndex = 7;
            this.BtnExport.Text = "Export";
            this.BtnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // BtnImport
            // 
            this.BtnImport.BackColor = System.Drawing.Color.Transparent;
            this.BtnImport.Centered = true;
            this.BtnImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnImport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnImport.Hover = true;
            this.BtnImport.Image = ((System.Drawing.Bitmap)(resources.GetObject("BtnImport.Image")));
            this.BtnImport.Location = new System.Drawing.Point(0, 0);
            this.BtnImport.Margin = new System.Windows.Forms.Padding(0);
            this.BtnImport.Name = "BtnImport";
            this.BtnImport.Size = new System.Drawing.Size(256, 24);
            this.BtnImport.TabIndex = 6;
            this.BtnImport.Text = "Import";
            // 
            // TexturePreview
            // 
            this.TexturePreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.TexturePreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TexturePreview.Location = new System.Drawing.Point(0, 40);
            this.TexturePreview.Name = "TexturePreview";
            this.TexturePreview.Size = new System.Drawing.Size(512, 472);
            this.TexturePreview.TabIndex = 7;
            this.TexturePreview.TabStop = false;
            // 
            // OSingleTextureWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.TexturePreview);
            this.Controls.Add(this.TopControls);
            this.Name = "OSingleTextureWindow";
            this.Size = new System.Drawing.Size(512, 512);
            this.Controls.SetChildIndex(this.TopControls, 0);
            this.Controls.SetChildIndex(this.TexturePreview, 0);
            this.TopControls.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TexturePreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel TopControls;
        private OButton BtnExport;
        private OButton BtnImport;
        private System.Windows.Forms.PictureBox TexturePreview;
    }
}
