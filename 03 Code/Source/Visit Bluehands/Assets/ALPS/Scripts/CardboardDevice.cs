
public enum CardboardType{
	DEFAULT,
	ALTERGAZE,
	CARDBOARD,
	FIREFLY
};

public enum ScreenOption{
	FixedSize, 
	FullScreen
};

public class CardboardDevice {

	public static ALPSConfig GetConfig(CardboardType device){
		ALPSConfig config;
		switch (device) {
			case CardboardType.ALTERGAZE:
			config = new ALPSConfig(CardboardType.ALTERGAZE,true,true,false,62f,62f,85f,-1f,0.4f,0.2f,0,0);
				break;
			case CardboardType.CARDBOARD:
			config = new ALPSConfig(CardboardType.CARDBOARD,true,true,false,62f,62f,85f,-1.5f,0.5f,0.2f,128,75);
				break;
			case CardboardType.FIREFLY:
			config = new ALPSConfig(CardboardType.FIREFLY,true,true,false,62f,62f,85f,-2f,0.7f,0.2f,140,75);
				break;
			case CardboardType.DEFAULT:
			default: 
				config = new ALPSConfig(CardboardType.DEFAULT,false,false,false,62f,62f,85f,0f,0f,0f,0,0);
				break;
		}
		return config;
	}
}
