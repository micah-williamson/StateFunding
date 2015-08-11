using System;

namespace StateFunding {
  public class Instance  {
    public Government Gov;

    [Persistent]
    public Review ActiveReview;

    [Persistent]
    public string govName;

    [Persistent]
    public int po;

    [Persistent]
    public int sc;

    [Persistent]
    Review[] Reviews = new Review[0];

    public Instance () {
      ActiveReview = new Review ();
    }

    public void addReview (Review R) {
      Review[] NewReviews = new Review[Reviews.Length+1];
      for(int i = 0; i < Reviews.Length; i++) {
        NewReviews[i] = Reviews[i];
      }
      NewReviews[NewReviews.Length-1] = R;

      Reviews = NewReviews;
    }

    public Review[] getReviews() {
      return Reviews;
    }
  }
}

