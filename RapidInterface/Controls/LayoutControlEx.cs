using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.Utils.Drawing;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Registrator;

namespace RapidInterface
{
    public class LayoutControlEx : LayoutControl
    {
        #region Methods

        protected override LayoutControlImplementor CreateILayoutControlImplementorCore()
        {
            return new LayoutControlImplementorEx(this);
        }

        #endregion
    }

    public class SkinGroupObjectPainterEx : SkinGroupObjectPainter
    {
        #region Constructors

        public SkinGroupObjectPainterEx(IPanelControlOwner owner, ISkinProvider provider)
            : base(owner, provider)
        {
        }

        #endregion

        #region Methods

        // Latest code from Stan @ DX.
        //protected override void DrawCaption(GroupObjectInfoArgs info)
        //{
        //   if (info.CaptionBounds.IsEmpty) return;
        //   info.Cache.FillRectangle(info.Cache.GetSolidBrush(Color.Aqua), info.CaptionBounds);
        //   info.Cache.DrawRectangle(info.Cache.GetPen(Color.Red), info.CaptionBounds);
        //   DrawButton(info);
        //   DrawVString(info.Cache, info.AppearanceCaption, info.Caption, info.TextBounds, GetRotateAngle(info));
        //}

        protected override void DrawCaption(GroupObjectInfoArgs info)
        {
            if (info.CaptionBounds.IsEmpty) return;
            info.Cache.FillRectangle(info.Cache.GetSolidBrush(info.Appearance.BackColor), info.CaptionBounds);
            DrawButton(info);
            DrawVString(info.Cache, info.AppearanceCaption, info.Caption, info.TextBounds, GetRotateAngle(info));
        }

        #endregion
    }

    public class LayoutControlImplementorEx : LayoutControlImplementor
    {
        #region Constructors

        public LayoutControlImplementorEx(ILayoutControlOwner owner)
            : base(owner)
        {
        }

        #endregion

        #region Methods

        protected override void InitializePaintStyles()
        {
            var lookAndFeelOwner = owner.GetISupportLookAndFeel();
            if (lookAndFeelOwner != null)
            {
                PaintStyles.Add(new LayoutOffice2003PaintStyle(lookAndFeelOwner));
                PaintStyles.Add(new LayoutWindowsXPPaintStyle(lookAndFeelOwner));
                //
                // TODO: Add your owner PaintStyle instead of PaintStyles.Add(new LayoutSkinPaintStyle(lookAndFeelOwner));
                //
                PaintStyles.Add(new LayoutSkinPaintStyleEx(lookAndFeelOwner));

                PaintStyles.Add(new Style3DPaintStyle(lookAndFeelOwner));
                PaintStyles.Add(new UltraFlatPaintStyle(lookAndFeelOwner));
                PaintStyles.Add(new FlatPaintStyle(lookAndFeelOwner));
            }
            lookAndFeelOwner = null;
        }

        #endregion
    }

    public class LayoutSkinPaintStyleEx : LayoutSkinPaintStyle
    {
        #region Constructors

        public LayoutSkinPaintStyleEx(ISupportLookAndFeel lookAndFeelOwner)
            : base(lookAndFeelOwner)
        {
        }

        #endregion

        #region Methods

        public override GroupObjectPainter CreateGroupPainter(IPanelControlOwner owner)
        {
            return new SkinGroupObjectPainterEx(owner, LookAndFeel);
        }

        #endregion
    }
}
