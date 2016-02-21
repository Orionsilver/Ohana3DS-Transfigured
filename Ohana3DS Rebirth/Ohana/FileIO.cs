﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

using Ohana3DS_Rebirth.Ohana;
using Ohana3DS_Rebirth.Ohana.ModelFormats;
using Ohana3DS_Rebirth.Ohana.ModelFormats.GenericFormats;
using Ohana3DS_Rebirth.Ohana.TextureFormats;
using Ohana3DS_Rebirth.Ohana.AnimationFormats;

namespace Ohana3DS_Rebirth.Ohana
{
    public class FileIO
    {
        public enum fileType
        {
            none,
            model,
            texture,
            light,
            camera,
            skeletalAnimation,
            materialAnimation,
            visibilityAnimation
        }

        /// <summary>
        ///     Imports a file of the given type.
        ///     Returns data relative to the chosen type.
        /// </summary>
        /// <param name="type">The type of the data</param>
        /// <returns></returns>
        public static Object import(fileType type)
        {
            using (OpenFileDialog openDlg = new OpenFileDialog())
            {
                switch (type)
                {
                    case fileType.model:
                        openDlg.Title = "Import Model";
                        openDlg.Filter = "Binary CTR H3D|*.bch|Binary CTR Model|*.bcres;*.bcmdl;*.cgfx|Source Model|*.smd";
                        openDlg.Multiselect = true;

                        if (openDlg.ShowDialog() == DialogResult.OK)
                        {
                            List<RenderBase.OModel> output = new List<RenderBase.OModel>();
                            foreach (string fileName in openDlg.FileNames)
                            {
                                try
                                {
                                    switch (openDlg.FilterIndex)
                                    {
                                        case 1: output.AddRange(BCH.load(fileName).model); break;
                                        case 2: output.AddRange(CGFX.load(fileName).model); break;
                                        case 3: output.AddRange(SMD.import(fileName).model); break;
                                    }
                                }
                                catch
                                {
                                    Debug.WriteLine("FileIO: Unable to import model file " + fileName + "!");
                                }
                            }
                            return output;
                        }
                        break;
                    case fileType.texture:
                        openDlg.Title = "Import Texture";
                        openDlg.Filter = "Binary CTR H3D|*.bch|Binary CTR Texture|*.bcres;*.bctex|Fantasy Life Texture|*.tex|Pokemon Texture|*.pt";
                        openDlg.Multiselect = true;

                        if (openDlg.ShowDialog() == DialogResult.OK)
                        {
                            List<RenderBase.OTexture> output = new List<RenderBase.OTexture>();
                            foreach (string fileName in openDlg.FileNames)
                            {
                                switch (openDlg.FilterIndex)
                                {
                                    case 1: output.AddRange(BCH.load(fileName).texture); break;
                                    case 2: output.AddRange(CGFX.load(fileName).texture); break;
                                    case 3: output.AddRange(ZTEX.load(fileName)); break;
                                    case 4: output.AddRange(PT.load(fileName)); break;
                                }
                            }
                            return output;
                        }
                        break;
                    case fileType.camera:
                        openDlg.Title = "Import Camera";
                        openDlg.Filter = "Binary CTR H3D|*.bch";
                        openDlg.Multiselect = true;

                        if (openDlg.ShowDialog() == DialogResult.OK)
                        {
                            List<RenderBase.OCamera> output = new List<RenderBase.OCamera>();
                            foreach (string fileName in openDlg.FileNames) output.AddRange(BCH.load(fileName).camera);
                            return output;
                        }
                        break;
                    case fileType.skeletalAnimation:
                        openDlg.Title = "Import Skeletal Animation";
                        openDlg.Filter = "Binary CTR H3D|*.bch|Source Model|*.smd|Pokémon Container Type F|*.pf|Pokémon Container Type B|*.pb|Pokémon Container Type K|*.pk";

                        if (openDlg.ShowDialog() == DialogResult.OK)
                        {
                            switch (openDlg.FilterIndex)
                            {
                                case 1: return BCH.load(openDlg.FileName).skeletalAnimation;
                                case 2: return SMD.import(openDlg.FileName).skeletalAnimation;
                                case 3: return PK.load(openDlg.FileName).skeletalAnimation;
                                case 4: return PK.load(openDlg.FileName).skeletalAnimation;
                                case 5: return PK.load(openDlg.FileName).skeletalAnimation;
                                default: return null;
                            }
                        }
                        break;
                    case fileType.materialAnimation:
                        openDlg.Title = "Import Material Animation";
                        openDlg.Filter = "Binary CTR H3D|*.bch|Pokémon Container Type F|*.pf|Pokémon Container Type B|*.pb|Pokémon Container Type K|*.pk";

                        if (openDlg.ShowDialog() == DialogResult.OK)
                        {
                            switch (openDlg.FilterIndex)
                            {
                                case 1: return BCH.load(openDlg.FileName).materialAnimation;
                                case 2: return PK.load(openDlg.FileName).materialAnimation;
                                case 3: return PK.load(openDlg.FileName).materialAnimation;
                                case 4: return PK.load(openDlg.FileName).materialAnimation;
                                default: return null;
                            }
                        }
                        break;
                    case fileType.visibilityAnimation:
                        openDlg.Title = "Import Visibility Animation";
                        openDlg.Filter = "Binary CTR H3D|*.bch|Pokémon Container Type F|*.pf|Pokémon Container Type B|*.pb|Pokémon Container Type K|*.pk";

                        if (openDlg.ShowDialog() == DialogResult.OK)
                        {
                            switch (openDlg.FilterIndex)
                            {
                                case 1: return BCH.load(openDlg.FileName).visibilityAnimation;
                                case 2: return PK.load(openDlg.FileName).visibilityAnimation;
                                case 3: return PK.load(openDlg.FileName).visibilityAnimation;
                                case 4: return PK.load(openDlg.FileName).visibilityAnimation;
                                default: return null;
                            }
                        }
                        break;
                }
            }

            return null;
        }

        /// <summary>
        ///     Exports a file of a given type.
        ///     Formats available to export will depend on the type of the data.
        /// </summary>
        /// <param name="type">Type of the data to be exported</param>
        /// <param name="data">The data</param>
        /// <param name="arguments">Optional arguments to be used by the exporter</param>
        public static void export(fileType type, Object data, List<int> arguments = null)
        {
            using (SaveFileDialog saveDlg = new SaveFileDialog())
            {
                if (arguments.Count > 2)
                {
                    saveDlg.FileName = arguments[2].ToString();

                    Directory.CreateDirectory(saveDlg.FileName);

                    for (int i = 0; i < ((RenderBase.OModelGroup)data).model.Count; i++)
                    {
                        OBJ.export(((RenderBase.OModelGroup)data), saveDlg.FileName + "/" + ((RenderBase.OModelGroup)data).model[i].name + ".obj", i);
                    }

                    foreach (RenderBase.OTexture texture in ((RenderBase.OModelGroup)data).texture)
                    {
                        if (File.Exists(saveDlg.FileName + "/" + texture.name + ".png"))
                        {
                            if (File.Exists(saveDlg.FileName + "/" + texture.name + "_2.png"))
                            {
                                texture.texture.Save(saveDlg.FileName + "/" + texture.name + "_3.png");
                            }
                            else
                            {
                                texture.texture.Save(saveDlg.FileName + "/" + texture.name + "_2.png");
                            }
                        }
                        else
                        {
                            texture.texture.Save(saveDlg.FileName + "/" + texture.name + ".png");
                        }
                    }
                }
                else
                {
                    switch (type)
                    {
                        case fileType.model:
                            saveDlg.Title = "Export Model";
                            saveDlg.Filter = "Source Model|*.smd";
                            saveDlg.Filter += "|Collada Model|*.dae";
                            saveDlg.Filter += "|Wavefront OBJ|*.obj";
                            saveDlg.Filter += "|CTR Model|*.cmdl";
                            if (saveDlg.ShowDialog() == DialogResult.OK)
                            {
                                switch (saveDlg.FilterIndex)
                                {
                                    case 1:
                                        SMD.export((RenderBase.OModelGroup)data, saveDlg.FileName, arguments[0]);
                                        break;
                                    case 2:
                                        DAE.export((RenderBase.OModelGroup)data, saveDlg.FileName, arguments[0]);
                                        break;
                                    case 3:
                                        OBJ.export((RenderBase.OModelGroup)data, saveDlg.FileName, arguments[0]);
                                        break;
                                    case 4:
                                        CMDL.export((RenderBase.OModelGroup)data, saveDlg.FileName, arguments[0]);
                                        break;
                                }
                            }
                            break;
                        case fileType.texture:
                            RenderBase.OModelGroup model = (RenderBase.OModelGroup)data;
                            saveDlg.Title = "Export Texture";
                            saveDlg.FileName = "dummy";

                            if (arguments[0] > -1)
                            {
                                saveDlg.Filter = "Selected texture|*.png|All textures|*.png";

                                if (saveDlg.ShowDialog() == DialogResult.OK)
                                {
                                    switch (saveDlg.FilterIndex)
                                    {
                                        case 1: model.texture[arguments[0]].texture.Save(saveDlg.FileName); break;
                                        case 2:
                                            foreach (RenderBase.OTexture texture in model.texture)
                                            {
                                                texture.texture.Save(Path.Combine(Path.GetDirectoryName(saveDlg.FileName), texture.name + ".png"));
                                            }
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                saveDlg.Filter = "All textures|*.png";

                                if (saveDlg.ShowDialog() == DialogResult.OK)
                                {
                                    foreach (RenderBase.OTexture texture in model.texture)
                                    {
                                        texture.texture.Save(Path.Combine(Path.GetDirectoryName(saveDlg.FileName), texture.name + ".png"));
                                    }
                                }
                            }

                            break;
                    }
                }
            }
        }
    }
}
