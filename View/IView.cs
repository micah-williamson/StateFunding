using System;

namespace StateFunding {
  public interface IView {
    void hide();
    bool isPainting();
    void paint();
    void show();
  }
}

