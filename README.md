实现了最基本的游戏功能，包括
1.主菜单
	1.0开始界面地图展示效果
	1.1舵式导航
		1.1.1点击不同按钮，切换不同功能面板组，同时隐藏其它的按钮对应地功能面板
			1.1.1.1功能上结构为：每个【面板】包含一系列的【按钮】，每个【按钮】又控制一组对应地【面板】。
		1.1.2首次点击导航栏，生成背景遮罩，遮蔽背景
	1.2游戏模式选择
		1.2.1【生存模式】和【寻宝模式】生成不同的地图
		1.2.2选择模式以后才能选择地图
	1.3游戏地图选择
		1.3.1点击游戏模式以后，根据【MapManager】挂载的地图数量，生成对应地地图
		1.3.2点击左右箭头，滑动选择地图
			1.3.2.1滑动有平移的动画效果
			1.3.2.2每次滑动动画固定时间为0.15s，每个点击动作都会入队，然后依次播放动画，直至队列空。
		1.3.3上下移动的关卡箭头，指示当前关卡
		1.3.4点击选择，进入游戏场景
	1.4弹出式面板
		1.4.1点击小按钮会呼出通知面板（如：日常任务）
			1.4.1.1有新的通知时，按钮右上方会限制红色感叹号提示，点击后叹号消失
			1.4.1.2弹出的面板会显示同时内容，提供一个【确认】按钮关闭面板，同时遮挡其背后所有点击。
			1.4.1.3弹出面板有缩放效果
			
2.游戏UI
	2.1主角状态栏
		2.1.1生命条：灰色底/绿色填充，显示当前值/最大值
		2.1.2经验条：灰色底/白色填充
		2.1.3头像框
	2.2数据栏
		2.2.1击杀敌人数：击杀敌人后增加
		2.2.2金币手机数：拾取金币后增加
		2.2.3当前敌人生成速率：个/每秒
	2.3计时器：显示当前生存时间
	2.4小地图：使用Rendering Texture+辅助相机完成
		2.4.1相机视场总是刚好覆盖地图
		2.4.2地图相机渲染【Minimap】图层（图标），被渲染Player，Enemy图层，不渲染阴影。
		2.4.3玩家的图标是方向箭头+同心圆，深色
		2.4.4敌人的图标时红点
		2.4.5小地图能够反应地图的变化
	2.5玩家属性面板：
		2.5.1此为临时面板，Debug用，显示玩家的ATK、DEF、武器等级，升级时更新
	2.6升级面板：
		2.6.1当玩家经验值达到时，呼出升级面板
		2.6.2升级面板呼出时，触发暂停，但不呼出暂停面板：TimeScale=0，CursorLockMode = None；
		2.6.3升级会弹出升级面板，其中有三个升级选项卡。
			2.6.3.1选项卡提供升级选项，并显示对应地文字，实现逻辑如下：
				2.6.3.1.1每个选项卡在Instanciate时，AddListener里有有一个函数容器，负责装在选项卡生成时装入对应地升级函数。
				2.6.3.1.2每个选项卡卡面上有个TMP，负责在选项卡生成时，提示其所装载的函数内容。
				2.6.3.1.3所有“升级函数”和其对应的“文字提示”以键值对的形式存在一个字典里，确保选项卡的文字和升级函数相对应。
				2.6.3.1.4所有升级函数又装在一个Action数组当中。
				2.6.3.1.5用一个从0开始的，长度等于升级函数数组的Int数组Indexes，每次需要生成选项卡时，以洗牌算法打乱，并抽取前三个数字，作为抽取升级函数数组的序号。
				2.6.3.1.6以此方法获得三组对应地函数和文字说明。当生成选项卡时，将函数装入容器，并将对应文字赋给卡面。
			2.6.3.2选项面板呼出时：
				2.6.3.2.1位于当前视觉树最前，功能上遮蔽后方所有可点击物，视觉上用半透黑色遮蔽游戏界面。
				2.6.3.2.2触发暂停：升级暂停另外触发UpgradePause，和普通的Pasue共同控制游戏暂停状态，避免在升级时点击暂停造成游戏继续。
			2.6.3.3选项卡在弹出时有 旋转一圈+上下移动的动画效果。此动画需的UpdateMode为RealTime，避免受到暂停时Timescale=0影响。
	2.7暂停面板：
		2.7.1游戏中按P随时呼出暂停面板
		2.7.2暂停面板提供两个按钮【继续】（游戏）和【退出】（到主菜单）
		2.7.3暂停时TimeScale=0，CursorLockMode = None；
	2.8游戏结束面板：
		2.8.1在玩家死亡时弹出。
		2.8.2展示的信息包括：GameOver，击杀敌人，生存时间
		2.8.3提供两个按钮：Restart 和 Mainmenu

		
		

 
