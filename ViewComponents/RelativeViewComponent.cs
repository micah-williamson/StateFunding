using System;
using UnityEngine;

namespace StateFunding {
  public class RelativeViewComponent: ViewComponent {
    protected ViewComponent Relative;
    protected float percentWidth = -1;
    protected float percentHeight = -1;
    protected float percentLeft = -1;
    protected float percentRight = -1;
    protected float percentTop = -1;
    protected float percentBottom = -1;

    public RelativeViewComponent () {}

    public void setRelativeTo(ViewComponent VC) {
      this.Relative = VC;
    }

    public bool hasRelative() {
      return this.Relative != null;
    }

    public void setPercentWidth(float width) {
      percentWidth = width / 100f;
    }

    public void setPercentHeight(float height) {
      percentHeight = height / 100f;
    }

    public void setPercentTop(float top) {
      percentTop = top / 100f;
    }

    public void setPercentBottom(float bottom) {
      percentBottom = bottom / 100f;
    }

    public void setPercentLeft(float left) {
      percentLeft = left / 100f;
    }

    public void setPercentRight(float right) {
      percentRight = right / 100f;
    }

    public override int getTopLeftX() {
      if(Relative.GetType() == typeof(ViewScroll)) {
        return base.getTopLeftX();
      }

      if (percentLeft != -1) {
        return (int)(percentLeft * Relative.getWidth() + Relative.getTopLeftX ());
      } else if (percentRight != -1) {
        return (int)(Relative.getBottomRightX () - percentRight * Relative.getWidth () - getWidth ());
      } else if (right != -1) {
        return (int)(Relative.getBottomRightX () - right - getWidth ());
      }

      return (int)(left + Relative.getTopLeftX ());
    }

    public override int getTopLeftY() {
      if(Relative.GetType() == typeof(ViewScroll)) {
        return base.getTopLeftY();
      }

      if (percentTop != -1) {
        return (int)(percentTop * Relative.getHeight () + Relative.getTopLeftY ());
      } else if (percentBottom != -1) {
        return (int)(Relative.getBottomRightY () - percentBottom * Relative.getHeight () - getHeight ());
      } else if (bottom != -1) {
        return (int)(Relative.getBottomRightY () - bottom - getHeight ());
      }
       
      return (int)(top + Relative.getTopLeftY ());
    }

    public override int getBottomRightX() {
      if(Relative.GetType() == typeof(ViewScroll)) {
        return base.getBottomRightX();
      }

      if (percentWidth != -1) {
        return (int)(percentWidth * Relative.getWidth() + getTopLeftX ());
      }

      return (int)(width + getTopLeftX ());
    }

    public override int getBottomRightY() {
      if(Relative.GetType() == typeof(ViewScroll)) {
        return base.getBottomRightY();
      }

      if (percentHeight != -1) {
        return (int)(percentHeight * Relative.getHeight() + getTopLeftY ());
      }

      return (int)(height + getTopLeftY ());
    }

    public override int getWidth () {
      if (percentWidth != -1) {
        return (int)(Relative.getWidth () * percentWidth);
      }

      return base.getWidth ();
    }

    public override int getHeight () {
      if (percentHeight != -1) {
        return (int)(Relative.getHeight () * percentHeight);
      }

      return base.getHeight ();
    }

    public override void paint() { }
  }
}

