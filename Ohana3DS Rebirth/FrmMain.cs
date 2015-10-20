﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

using Ohana3DS_Rebirth.Ohana;
using Ohana3DS_Rebirth.Ohana.ModelFormats;
using Ohana3DS_Rebirth.Ohana.TextureFormats;
using Ohana3DS_Rebirth.Ohana.AnimationFormats;

namespace Ohana3DS_Rebirth
{
    public partial class FrmMain : OForm
    {
        public FrmMain()
        {
            InitializeComponent();
            WindowManager.initialize(DockContainer);
            MainMenu.Renderer = new GUI.OMenuStrip();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            WindowManager.flush();
        }

        private void LblTitle_MouseDown(object sender, MouseEventArgs e)
        {
            MainMenu.Show(Left + LblTitle.Left, Top + LblTitle.Top + LblTitle.Height);
            if (e.Button == MouseButtons.Left) MainMenu.Show(Left + LblTitle.Left, Top + LblTitle.Top + LblTitle.Height);
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ohana3DS Rebirth made by gdkchan. Additional modifications by Quibilia.", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void mnuOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openDlg = new OpenFileDialog())
            {
                openDlg.Filter = "All supported files|*.bch;*.cx;*.lz;*.cmp;*.mm;*.gr;*.pc;*.pack;*.fpt;*.dmp;*.rel;*.bcres;*.bcmdl;*.bctex;*.mdl;*.tex;*.pt;*.pk;*.pb;*.pf";
                openDlg.Filter += "|Binary CTR H3D|*.bch";
                openDlg.Filter += "|Compressed file|*.cx;*.lz;*.cmp";
                openDlg.Filter += "|Pokémon Overworld model|*.mm";
                openDlg.Filter += "|Pokémon Map model|*.gr";
                openDlg.Filter += "|Pokémon Species model|*.pc";
                openDlg.Filter += "|Pokémon Species texture|*.pt";
                openDlg.Filter += "|Pokémon Material animation|*.pf";
                openDlg.Filter += "|Pokémon Skeleton animation|*.pb";
                openDlg.Filter += "|Pokémon Visibility animation|*.pk";
                openDlg.Filter += "|Dragon Quest VII Package|*.pack";
                openDlg.Filter += "|Dragon Quest VII Container|*.fpt";
                openDlg.Filter += "|Dragon Quest VII Texture|*.dmp";
                openDlg.Filter += "|Forbidden Magna CGFX|*.rel";
                openDlg.Filter += "|Binary CTR Resource|*.bcres";
                openDlg.Filter += "|Binary CTR Model|*.bcmdl";
                openDlg.Filter += "|Binary CTR Texture|*.bctex";
                openDlg.Filter += "|Fantasy Life Model|*.mdl";
                openDlg.Filter += "|Fantasy Life Texture|*.tex";
                openDlg.Filter += "|All files|*.*";
                if (openDlg.ShowDialog() != DialogResult.OK) return;
                open(openDlg.FileName);
            }
        }

        public void open(string fileName)
        {
            string name = Path.GetFileNameWithoutExtension(fileName);
            open(new FileStream(fileName, FileMode.Open), name);
        }

        public void open(Stream data, string name)
        {
            WindowManager.flush();

            FileIdentifier.fileFormat format = FileIdentifier.identify(data);
            if (FileIdentifier.isCompressed(format)) CompressionManager.decompress(ref data, ref format);

            byte[] buffer;
            OContainerForm containerForm;
            switch (format)
            {
                case FileIdentifier.fileFormat.H3D:
                    buffer = new byte[data.Length];
                    data.Read(buffer, 0, buffer.Length);
                    data.Close();
                    launchModel(BCH.load(new MemoryStream(buffer)), name);
                    break;
                case FileIdentifier.fileFormat.PkmnContainer:
                    Ohana.Containers.GenericContainer.OContainer container = Ohana.Containers.PkmnContainer.load(data);
                    data.Seek(0, SeekOrigin.Begin);
                    switch (container.fileIdentifier)
                    {
                        case "PC": //Pokémon model
                            launchModel(BCH.load(new MemoryStream(container.content[0].data)), name);
                            break;
                        case "MM": //Pokémon Overworld model
                            launchModel(BCH.load(new MemoryStream(container.content[0].data)), name);
                            break;
                        case "GR": //Pokémon Map model
                            launchModel(BCH.load(new MemoryStream(container.content[1].data)), name);
                            break;
                        case "PT":
                            GUI.OTextureWindow ptTextureWindow = new GUI.OTextureWindow();
                            ptTextureWindow.Title = name;
                            
                            launchWindow(ptTextureWindow);
                            DockContainer.dockMainWindow();
                            WindowManager.Refresh();
                            
                            ptTextureWindow.initialize(PT.load(data));
                            break;
                        case "PF":
                            buffer = new byte[data.Length];
                            data.Read(buffer, 0, buffer.Length);
                            data.Close();
                            launchModel(PK.load(new MemoryStream(buffer)), name);
                            break;
                        case "PK":
                            buffer = new byte[data.Length];
                            data.Read(buffer, 0, buffer.Length);
                            data.Close();
                            launchModel(PK.load(new MemoryStream(buffer)), name);
                            break;
                        case "PB":
                            buffer = new byte[data.Length];
                            data.Read(buffer, 0, buffer.Length);
                            data.Close();
                            launchModel(PK.load(new MemoryStream(buffer)), name);
                            break;
                    }
                    //TODO: Add windows for extra data

                    break;
                case FileIdentifier.fileFormat.CGFX:
                    buffer = new byte[data.Length];
                    data.Read(buffer, 0, buffer.Length);
                    data.Close();
                    launchModel(CGFX.load(new MemoryStream(buffer)), name);
                    break;
                case FileIdentifier.fileFormat.zmdl:
                    buffer = new byte[data.Length];
                    data.Read(buffer, 0, buffer.Length);
                    data.Close();
                    launchModel(ZMDL.load(new MemoryStream(buffer)), name);
                    break;
                case FileIdentifier.fileFormat.ztex:
                    GUI.OTextureWindow textureWindow = new GUI.OTextureWindow();

                    textureWindow.Title = name;

                    launchWindow(textureWindow);
                    DockContainer.dockMainWindow();
                    WindowManager.Refresh();

                    textureWindow.initialize(ZTEX.load(data));
                    break;
                case FileIdentifier.fileFormat.DQVIIPack:
                    containerForm = new OContainerForm();
                    containerForm.launch(Ohana.Containers.DQVIIPack.load(data));
                    containerForm.Show(this);
                    break;
                case FileIdentifier.fileFormat.FPT0:
                    containerForm = new OContainerForm();
                    containerForm.launch(Ohana.Containers.FPT0.load(data));
                    containerForm.Show(this);
                    break;
                case FileIdentifier.fileFormat.NLK2:
                    buffer = new byte[data.Length - 0x80];
                    data.Seek(0x80, SeekOrigin.Begin);
                    data.Read(buffer, 0, buffer.Length);
                    data.Close();
                    launchModel(CGFX.load(new MemoryStream(buffer)), name);
                    break;
                case FileIdentifier.fileFormat.DMPTexture:
                    GUI.OSingleTextureWindow singleTextureWindow = new GUI.OSingleTextureWindow();

                    singleTextureWindow.Title = name;

                    launchWindow(singleTextureWindow);
                    DockContainer.dockMainWindow();
                    WindowManager.Refresh();

                    singleTextureWindow.initialize(DMP.load(data).texture);
                    break;
                default:
                    MessageBox.Show("Unsupported file format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    data.Close();
                    break;
            }
        }

        /// <summary>
        ///     Opens the windows used for a model.
        ///     More windows can be opened later, for files with model and other data.
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="name">The file name (without the full path and extension)</param>
        private void launchModel(RenderBase.OModelGroup model, string name)
        {
            GUI.OViewportWindow viewportWindow = new GUI.OViewportWindow();
            GUI.OModelWindow modelWindow = new GUI.OModelWindow();
            GUI.OTextureWindow textureWindow = new GUI.OTextureWindow();
            GUI.OLightWindow lightWindow = new GUI.OLightWindow();
            GUI.OCameraWindow cameraWindow = new GUI.OCameraWindow();
            GUI.OAnimationsWindow animationWindow = new GUI.OAnimationsWindow();

            viewportWindow.Title = name;
            modelWindow.Title = "Models";
            textureWindow.Title = "Textures";
            lightWindow.Title = "Lights";
            cameraWindow.Title = "Cameras";
            animationWindow.Title = "Animations";

            RenderEngine renderer = new RenderEngine();
            renderer.model = model;

            launchWindow(viewportWindow);
            DockContainer.dockMainWindow();
            launchWindow(modelWindow, false);
            launchWindow(textureWindow, false);
            launchWindow(lightWindow, false);
            launchWindow(cameraWindow, false);
            launchWindow(animationWindow, false);

            WindowManager.Refresh();

            modelWindow.initialize(renderer);
            textureWindow.initialize(renderer);
            lightWindow.initialize(renderer);
            cameraWindow.initialize(renderer);
            animationWindow.initialize(renderer);
            viewportWindow.initialize(renderer);
        }

        private void launchWindow(GUI.ODockWindow window, bool visible = true)
        {
            window.Visible = visible;
            DockContainer.launch(window);
            WindowManager.addWindow(window);
        }

        private void FrmMain_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            System.Collections.Generic.List<int> args;
            RenderBase.OModelGroup group;
            Ohana.Containers.GenericContainer.OContainer container;

            for (int i = 0; i < files.Length; i++)
            {
                args = new System.Collections.Generic.List<int>();
                group = new RenderBase.OModelGroup();

                try
                {
                    if (files[i].Contains(".pc"))
                    {
                        container = Ohana.Containers.PkmnContainer.load(files[i]);
                        group = BCH.load(new MemoryStream(container.content[0].data));
                    }
                    else if (files[i].Contains(".pt"))
                    {
                        group.texture = PT.load(files[i]);
                    }
                    else if (files[i].Contains(".pk"))
                    {
                        group.visibilityAnimation = PK.load(files[i]).visibilityAnimation;
                        group.skeletalAnimation = PK.load(files[i]).skeletalAnimation;
                        group.materialAnimation = PK.load(files[i]).materialAnimation;
                    }
                    else if (files[i].Contains(".pb"))
                    {
                        group.materialAnimation = PK.load(files[i]).materialAnimation;
                        group.visibilityAnimation = PK.load(files[i]).visibilityAnimation;
                        group.skeletalAnimation = PK.load(files[i]).skeletalAnimation;
                    }
                    else if (files[i].Contains(".pf"))
                    {
                        group.skeletalAnimation = PK.load(files[i]).skeletalAnimation;
                        group.materialAnimation = PK.load(files[i]).materialAnimation;
                        group.visibilityAnimation = PK.load(files[i]).visibilityAnimation;
                    }
                    else if (files[i].Contains(".lz"))
                    {
                        FileIdentifier.fileFormat fmt = FileIdentifier.fileFormat.LZSSCompressed;
                        Stream str = new FileStream(files[i], FileMode.OpenOrCreate);

                        CompressionManager.decompress(ref str, ref fmt);

                        Stream outStr = new FileStream(files[i] + ".PKj", FileMode.OpenOrCreate);

                        byte[] outBuf = new byte[str.Length];
                        str.Read(outBuf, 0, (int)outBuf.Length);
                        str.Close();

                        outStr.Write(outBuf, 0, (int)outBuf.Length);

                        outStr.Close();
                    }
                    else if (files[i].Contains(".CGFX"))
                    {
                        group.visibilityAnimation = CGFX.load(files[i]).visibilityAnimation;
                        group.skeletalAnimation = CGFX.load(files[i]).skeletalAnimation;
                        group.materialAnimation = CGFX.load(files[i]).materialAnimation;
                    }
                    else
                    {
                        group = BCH.load(files[i]);
                    }

                    if (group.model.Count > 0)
                    {
                        FileIO.export(FileIO.fileType.model, group, args);
                    }

                    if (group.texture.Count > 0)
                    {
                        args.Add(i);
                        args.Add(0);
                        args.Add(0);
                        FileIO.export(FileIO.fileType.texture, group, args);
                    }
                }
                catch
                {
                }
            }
        }

        private void FrmMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void DockContainer_Click(object sender, EventArgs e)
        {

        }
    }
}
