using LiteGui.Graphics;

namespace LiteGui.Control
{
    internal static class LGuiTexture
    {
        internal static void OnProcess(int TextureID, LGuiRect SrcRect, LGuiVec2 DstSize)
        {
            var ID = LGuiHash.CalculateID(LGuiSettings.DefaultTextureTitle + TextureID);
            LGuiContext.SetPreviousControlID(ID);

            var DstRect = LGuiLayout.DoLayout(DstSize);
            if (!LGuiMisc.CheckVisible(ref DstRect))
            {
                return;
            }

            LGuiMisc.CheckAndSetContextID(ref DstRect, ID, true);

            LGuiGraphics.DrawTexture(TextureID, SrcRect, DstRect);
        }
    }
}