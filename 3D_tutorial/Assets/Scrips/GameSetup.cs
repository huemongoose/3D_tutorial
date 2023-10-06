using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    int redBallsRemaining = 7;
    int blueBallsRemaining = 7;
    float ballRadius;
    float ballDiameter;

    [SerializeField] GameObject ballPrefab;
    [SerializeField] Transform cueBallPosition;
    [SerializeField] Transform headBallPosition;


    private void Awake()
    {
        ballRadius = ballPrefab.GetComponent<SphereCollider>().radius * 100f;
        ballDiameter = ballRadius * 2;
        PlaceallBalls();

    }
    // Start is called before the first frame updat
    void PlaceallBalls()
    {
        PlacecueBall();
        placeRandomBalls();

    }
    void PlacecueBall()
    {
        GameObject ball = Instantiate(ballPrefab, cueBallPosition.position, Quaternion.identity);
        ball.GetComponent<Ball>().MakeCueBall();
    }
    void PlaceEightBall(Vector3 position)
    {
        GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
        ball.GetComponent<Ball>().MakeEightBall();
    }
    void placeRandomBalls()
    {
        int NuminthisRow = 1;
        int rand;
        Vector3 firstinRowPosition = headBallPosition.position;
        Vector3 currentPosition = firstinRowPosition;

        void PlaceRedBall(Vector3 position)
        {
            GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
            ball.GetComponent<Ball>().BallsetUp(true);
            redBallsRemaining--;
        }
        void PlaceBlueBall(Vector3 position)
        {
            GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
            ball.GetComponent<Ball>().BallsetUp(false);
            blueBallsRemaining--;
        }

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < NuminthisRow; j++)
            {
                if (i == 2 && j == 1)
                {
                    PlaceEightBall(currentPosition);
                }
                else if (redBallsRemaining > 0 && blueBallsRemaining > 0)
                {
                    rand = Random.Range(0, 2);
                    if (rand == 0)
                    {
                        PlaceRedBall(currentPosition);
                    }
                    else
                    {
                        PlaceBlueBall(currentPosition);
                    }
                }
                else if (redBallsRemaining > 0)
                {
                    PlaceRedBall(currentPosition);
                }
                else
                {
                    PlaceBlueBall(currentPosition);
                }
                currentPosition += new Vector3(1, 0, 0).normalized * ballDiameter;
            }
            firstinRowPosition += Vector3.back * (Mathf.Sqrt(3) * ballRadius) + Vector3.left * ballRadius;
            currentPosition = firstinRowPosition;
            NuminthisRow++;
        }

    }
}
