public class DataReward {

	public int id = 0;
	public int count = 0;

	public DataReward(int itemID, int cnt) {
		id = itemID;
		count = cnt;
	}
	public DataReward(DataReward rew) {
		id = rew.id;
		count = rew.count;
	}
	
	public void reset() {
		id = -1;
		count = 0;
	}

}
