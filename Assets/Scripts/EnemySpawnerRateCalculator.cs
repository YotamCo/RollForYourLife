

class EnemySpawnerRateCalculator
{
    enum SpawningStates{
        INITIAL_STATE,
        FINAL_STATE
    }
    /*

    private int _levelTargetScore;
    private float _initialTimeBetweenSpawns;
    private SpawningStates _currentState = INITIAL_STATE;

    public void Init(float timeBetweenSpawns, int levelTargetScore)
    {
        _initialTimeBetweenSpawns = timeBetweenSpawns;
        _levelTargetScore = levelTargetScore;
    }

    public float CalculateSpawnRate(int totalScore)
    {
        int timeBetweenSpawns;
        switch (SpawningStates _currentState)
        {
            case INITIAL_STATE:
                if(totalScore >= _levelTargetScore / 2)
                {
                    _currentState = FINAL_STATE;
                    timeBetweenSpawns = _initialTimeBetweenSpawns / 2;
                }
                timeBetweenSpawns = _initialTimeBetweenSpawns;
                break;

            case FINAL_STATE:
                timeBetweenSpawns = _initialTimeBetweenSpawns / 2;
                break;
        }

        return timeBetweenSpawns;
    }*/
}