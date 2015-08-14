using System;

namespace StateFunding {
  public static class TimeHelper {
    public static int Minutes(double s) {
      return (int)(s / 60);
    }

    public static int Hours(double s) {
      return (int)(s / 60 / 60);
    }

    public static int Days(double s) {
      return (int)(s / 60 / 60 / 6);
    }

    public static int Months(double s) {
      return (int)(s / 60 / 60 / 6 / 6.45);
    }

    public static int Years(double s) {
      return (int)(s / 60 / 60 / 6 / 426);
    }

    public static int Quarters(double s) {
      return (int)(s / 60 / 60 / 6 / 426 * 4);
    }

    public static int ToMinutes(int s) {
      return (int)(s * 60);
    }

    public static int ToHours(int s) {
      return (int)(s * 60 * 60);
    }

    public static int ToDays(int s) {
      return (int)(s * 60 * 60 * 6);
    }

    public static int ToMonths(int s) {
      return (int)(s * 60 * 60 * 6 * 6.45);
    }

    public static int ToYears(int s) {
      return (int)(s * 60 * 60 * 6 * 426);
    }

    public static int ToQuarters(int s) {
      return (int)(s * 60 * 60 * 6 * 426 * 4);
    }
  }
}

