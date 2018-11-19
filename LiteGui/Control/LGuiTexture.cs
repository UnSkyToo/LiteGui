using LiteGui.Graphics;

namespace LiteGui.Control
{
    internal static class LGuiTexture
    {
        internal static void OnProcess(int TextureID, LGuiRect SrcRect, LGuiVec2 DstSize)
        {
            var ID = LGuiHash.CalculateID(LGuiSettings.DefaultTextureTitle + TextureID);
            LGuiContext.SetPreviousControlID(ID);

            if (SrcRect.Width == 0 || SrcRect.Height == 0)
            {
                SrcRect.Size = LGuiConvert.GetTextureIDSize(TextureID);
            }

            if (DstSize.X == 0 || DstSize.Y == 0)
            {
                DstSize = LGuiConvert.GetTextureIDSize(TextureID);
            }

            var DstRect = LGuiLayout.DoLayout(DstSize);
            if (!LGuiMisc.CheckVisible(ref DstRect))
            {
                return;
            }

            LGuiMisc.CheckAndSetContextID(ref DstRect, ID, true);

            LGuiGraphics.DrawTexture(TextureID, SrcRect, DstRect);
        }

        internal static void OnProcess(string FilePath, LGuiRect SrcRect, LGuiVec2 DstSize)
        {
            var ID = LGuiHash.CalculateID(FilePath);
            LGuiContext.SetPreviousControlID(ID);

            if (SrcRect.Width == 0 || SrcRect.Height == 0)
            {
                SrcRect.Size = LGuiConvert.GetTexturePathSize(FilePath);
            }

            if (DstSize.X == 0 || DstSize.Y == 0)
            {
                DstSize = LGuiConvert.GetTexturePathSize(FilePath);
            }

            var DstRect = LGuiLayout.DoLayout(DstSize);
            if (!LGuiMisc.CheckVisible(ref DstRect))
            {
                return;
            }

            LGuiMisc.CheckAndSetContextID(ref DstRect, ID, true);

            LGuiGraphics.DrawTexture(FilePath, SrcRect, DstRect);
        }
    }
}