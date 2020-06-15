using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class snake : MonoBehaviour {
	
	class Node
	{
		public float x;
		public float y;
		public Node next;
		public Node prev;
		
		public Node(float x,float y)
		{
			this.x = x;
			this.y = y;
		}
		
		public Node(float x,float y,Node prev)
		{
			this.x = x;
			this.y = y;
			this.prev = prev;
		}
	}
	
	class List
	{
		public int size;
		public Node head;
		public Node tail;
		
		public List()
		{
			this.size = 0;
			this.head = new Node(0,0);
			this.tail = new Node(0,0);
		}
		
		public void addNode(float x,float y)
		{
			Node temp = this.head;
			int counter = 0;
			while(counter < size)
			{
				temp = temp.next;
				counter++;
			}
			this.tail.prev = temp.next = new Node(x,y,temp);
			
			this.size++;
		}
		
		public Node deleteFirst()
		{
			Node temp = this.head.next;
			
			this.head.next = this.head.next.next;
			
			this.size--;
			
			return temp;
		}
		
		public Node deleteLast()
		{
			Node temp = this.tail.prev;
			
			this.tail.prev = this.tail.prev.prev;
			
			this.size--;
			
			return temp;
		}
	}
	
	
	
	public GameObject apple;
	public GameObject SnakePart;
	
	public GameObject SnakeHeadUp;
	public GameObject SnakeHeadDown;
	public GameObject SnakeHeadRight;
	public GameObject SnakeHeadLeft;
	
	public GameObject tailUp;
	public GameObject tailDown;
	public GameObject tailLeft;
	public GameObject tailRight;
	
	public GameObject snakeStarightUp;
	public GameObject snakeStarightForward;
	public GameObject snakeTurnUpLeft;
	public GameObject snakeTurnUpRight;
	public GameObject snakeTurnDownLeft;
	public GameObject snakeTurnDownRight;
	
	
	//public GameObject AppleParticle;
	
	public Text length;
	
	GameObject prevApple;
	GameObject prevSankePart;
	
	public Vector3 snakePosition = new Vector3(0.5f,0.5f,0f);
	
	bool up    = false;
	bool down  = false;
	bool left  = false;
	bool right = false;
	
	bool start = false;
	
	//float times = 2f;
	
	float appleX;
	float appleY;
	
	bool appleCreated = false;
	
	bool pause = false;
	
	int Speed = 8;
	int Nitro = 15;
	
	List Snake = new List();
	LinkedList<GameObject> parts = new LinkedList<GameObject>();
	
	bool BorderCollision  = false;
	bool SnakeCollision   = false;
	
	
	void deleteOldSnake()
	{
		int tempCount = parts.Count;
		for(int i=0;i<tempCount;i++)
		{
			prevSankePart = (GameObject)parts.First.Value;
			parts.RemoveFirst();
			Destroy(prevSankePart);
		}
	}
	
	void respawnApple()
	{
		appleX = Random.Range(-12, 12) + 0.5f;
		appleY = Random.Range(-7, 4) + 0.5f;
		
		//print("appleX : " + appleX);
		//print("appleY : " + appleY);
		Node temp = Snake.head;
		int counter = 0;
		while( counter < Snake.size )
		{
			temp = temp.next;
			
			if(temp.x == appleX && temp.y == appleY)
			{
				respawnApple();
			}
			counter++;
		}
		
		appleCreated = false;
	}
	
	void makeAllFalse()
	{
		up = down = left = right = false;
	}
	
	bool checkBorderCollision()
	{
		if( snakePosition.x >= 17 || snakePosition.x <= -16 || snakePosition.y >= 6 || snakePosition.y <= -9 )
		{
			return true;
		}
		
		return false;
	}
	
	bool checkSnakeCollision()
	{
		Node temp = Snake.head;
		int counter = 0;
		while( counter < Snake.size )
		{
			temp = temp.next;
			
			if( temp.x == snakePosition.x && temp.y == snakePosition.y )
			{
				return true;
			}
			
			counter++;
		}
		
		return false;
	}
	
	bool checkSnakeAndAppleCollision()
	{
		Node temp = Snake.tail;
		int counter = 0;
		while( counter < Snake.size )
		{
			temp = temp.prev;
			
			if( temp.x == appleX && temp.y == appleY )
			{
				return true;
			}
			
			counter++;
		}
		
		return false;
	}
	
	public void WriteString()
	{
		string path = "save.ini";
		
		File.Delete(path);
		
		//Write some text to the test.txt file
		StreamWriter writer = new StreamWriter(path, true);
		
		Node temp = Snake.head;
		int counter = 0;
		while( counter < Snake.size )
		{
			temp = temp.next;
			
			writer.WriteLine(temp.x + " " + temp.y);
			
			counter++;
		}
		//writer.WriteLine("TEST");
		writer.Close();
	}
	
	public void ReadString()
	{
		string path = "save.ini";

		string line;
		//Read the text from directly from the test.txt file
		StreamReader reader = new StreamReader(path); 
		
		string[] arr ;
		
		start = false;
			
			
		deleteOldSnake();
		Destroy(prevApple);
		
		//snakePosition = new Vector3(0.5f,0.5f,0f);
		Snake = new List();
		makeAllFalse();
		BorderCollision  = false;
		SnakeCollision = false;
		
		do
        {
            line = reader.ReadLine();
			if(line == null)
			{
				break;
			}
			
			arr = line.Split(" "[0]);
			
			
			Snake.addNode( float.Parse(arr[0]), float.Parse(arr[1]) );

            //Console.WriteLine(text);
            //print (arr[0] + ", " + arr[1]);
			
        } while (line != null);
		
		snakePosition.x = Snake.tail.prev.x;
		snakePosition.y = Snake.tail.prev.y;
		
		appleCreated = false;
		respawnApple();
		//Debug.Log(reader.ReadToEnd());
		reader.Close();
     }
	
	public void exit()
	{
		Application.Quit();
	}
	
	// Use this for initialization
	void Start () {
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = Speed;
		
		respawnApple();
		
		transform.position = snakePosition;
		
		Snake.addNode(snakePosition.x-2,snakePosition.y);
		Snake.addNode(snakePosition.x-1,snakePosition.y);
		Snake.addNode(snakePosition.x,snakePosition.y);
		
		GetComponent<Renderer>().enabled = false;
		
		//WriteString();
	}
	
	// Update is called once per frame
	void Update () {
		
		length.text = "Length : " + Snake.size + "";
		
		
		if(SnakeCollision == false && BorderCollision == false)
		{
			if ( Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) )
			{
				if( up == true )
				{
					Application.targetFrameRate = Nitro;
				}
				
				if( down == false )
				{
					makeAllFalse();
					up = true;
				}
				
				start = true;
				
				if(pause == true)
					pause = false;
			} 
			else if ( Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) )
			{
				if( down == true )
				{
					Application.targetFrameRate = Nitro;
				}
				
				if( up == false )
				{
					makeAllFalse();
					down = true;
				}

				start = true;
				
				if(pause == true)
					pause = false;
			} 
			else if ( Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) )
			{
				if( right == true )
				{
					Application.targetFrameRate = Nitro;
				}
				
				if( left == false )
				{
					makeAllFalse();
					right = true;
				}

				start = true;
				
				if(pause == true)
					pause = false;
			} 
			else if ( Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) )
			{
				if( left == true )
				{
					Application.targetFrameRate = Nitro;
				}
				
				if( right == false && start == true )
				{
					makeAllFalse();
					left = true;
				}
				//start = true;
				if(pause == true)
					pause = false;
			}
			else if (Input.GetKey(KeyCode.Space))
			{
				pause = !pause;
			}
			else
			{
				Application.targetFrameRate = Speed;			
			}
		
		
		}
		
		if ( Input.GetKey(KeyCode.Escape) )
        {
			start = false;
			
			
			deleteOldSnake();
			Destroy(prevApple);
			
			snakePosition = new Vector3(0.5f,0.5f,0f);
			Snake = new List();
			makeAllFalse();
			BorderCollision  = false;
			SnakeCollision = false;
			
			Snake.addNode(snakePosition.x-2,snakePosition.y);
			Snake.addNode(snakePosition.x-1,snakePosition.y);
			Snake.addNode(snakePosition.x,snakePosition.y);
			
			
			
			
			
			appleCreated = false;
			respawnApple();
			//GetComponent<Renderer>().enabled = true;
        }
		else if( Input.GetKey(KeyCode.Return) )
		{
			WriteString();
			//ReadString();
		}
		
		
		
		
		if( start == true && pause == false && BorderCollision == false && SnakeCollision == false)
		{
			
			
			if( up == true )
			{
				snakePosition.y += 1f;
				//transform.position = snakePosition;
				//transform.Translate( Vector3.up );
			} else if( down == true )
			{
				snakePosition.y -= 1f;
				//transform.position = snakePosition;
				//transform.Translate( Vector3.down );
			} else if( right == true )
			{
				snakePosition.x += 1f;
				//transform.position = snakePosition;
				//transform.Translate( Vector3.right );
			} else if( left == true )
			{
				snakePosition.x -= 1f;
				//transform.position = snakePosition;
				//transform.Translate( Vector3.left );
			}
			
			
			
			//prevSankePart = Instantiate(SnakePart, new Vector3(snakePosition.x, snakePosition.y, 0), Quaternion.identity);
			
			
			
			
			//if(snakePosition.x == appleX && snakePosition.y == appleY)
			if( checkSnakeAndAppleCollision() == true)
			{
				//GameObject particle = Instantiate(AppleParticle,new Vector3(prevApple.transform.position.x,prevApple.transform.position.y,-10),Quaternion.identity);
				//Destroy(particle,1f);
				Destroy(prevApple);
				respawnApple();
			}
			else
			{
				Snake.deleteFirst();
			}
			
			
			BorderCollision = checkBorderCollision();
			
			SnakeCollision = checkSnakeCollision();
			
			Snake.addNode(snakePosition.x,snakePosition.y);

			
		}
		else
		{
			//makeAllFalse();
		}
		
		int tempCount = parts.Count;
		for(int i=0;i<tempCount;i++)
		{
			prevSankePart = (GameObject)parts.First.Value;
			parts.RemoveFirst();
			Destroy(prevSankePart);
		}
		
		Node temp = Snake.head;
		int counter = 0;
		while(counter < Snake.size)
		{
			temp = temp.next;
			
			
			if( counter == 0 )
			{
				if( temp.next.x > temp.x )
				{
					prevSankePart = Instantiate(tailRight, new Vector3(temp.x, temp.y, -0.9f), Quaternion.identity);
				} else if( temp.next.x < temp.x )
				{
					prevSankePart = Instantiate(tailLeft, new Vector3(temp.x, temp.y, -0.9f), Quaternion.identity);
				} else if( temp.next.y > temp.y )
				{
					prevSankePart = Instantiate(tailUp, new Vector3(temp.x, temp.y, -0.9f), Quaternion.identity);
				} else if( temp.next.y < temp.y )
				{
					prevSankePart = Instantiate(tailDown, new Vector3(temp.x, temp.y, -0.9f), Quaternion.identity);
				}
			}
			else if(counter == Snake.size-1)
			{
				//if( up == true )
				if( temp.y > temp.prev.y )				
				{
					prevSankePart = Instantiate(SnakeHeadUp, new Vector3(temp.x, temp.y, -0.9f), Quaternion.identity);
				} 
				//else if( down == true )
				else if( temp.y < temp.prev.y )
				{
					prevSankePart = Instantiate(SnakeHeadDown, new Vector3(temp.x, temp.y, -0.9f), Quaternion.identity);
				} 
				//else if( left == true )
				else if( temp.x < temp.prev.x )
				{
					prevSankePart = Instantiate(SnakeHeadLeft, new Vector3(temp.x, temp.y, -0.9f), Quaternion.identity);
				} 
				//else if( right == true )
				else if( temp.x > temp.prev.x )
				{
					prevSankePart = Instantiate(SnakeHeadRight, new Vector3(temp.x, temp.y, -0.9f), Quaternion.identity);
				} 
				else
				{
					//prevSankePart = Instantiate(SnakeHeadRight, new Vector3(temp.x, temp.y, -0.9f), Quaternion.identity);
				}
				
			}
			else
			{
				if( temp.next.y == temp.y && temp.prev.y == temp.y)
				{
					prevSankePart = Instantiate(snakeStarightForward, new Vector3(temp.x, temp.y, -0.9f), Quaternion.identity);
				} else if( temp.next.y == temp.y && temp.prev.y == temp.y )
				{
					prevSankePart = Instantiate(snakeStarightForward, new Vector3(temp.x, temp.y, -0.9f), Quaternion.identity);
				} else if( temp.next.x == temp.x && temp.prev.x == temp.x )
				{
					prevSankePart = Instantiate(snakeStarightUp, new Vector3(temp.x, temp.y, -0.9f), Quaternion.identity);
				} else if( temp.next.x == temp.x && temp.prev.x == temp.x )
				{
					prevSankePart = Instantiate(snakeStarightUp, new Vector3(temp.x, temp.y, -0.9f), Quaternion.identity);
				} 
								
				else if( temp.next.y == temp.y && temp.prev.y > temp.y && temp.next.x > temp.x )
				{
					prevSankePart = Instantiate(snakeTurnUpLeft, new Vector3(temp.x, temp.y, -0.9f), Quaternion.identity);
				} else if( temp.next.y == temp.y && temp.prev.y > temp.y && temp.next.x < temp.x )
				{
					prevSankePart = Instantiate(snakeTurnUpRight, new Vector3(temp.x, temp.y, -0.9f), Quaternion.identity);
				} else if( temp.next.y == temp.y && temp.prev.y < temp.y && temp.next.x > temp.x )
				{
					prevSankePart = Instantiate(snakeTurnDownLeft, new Vector3(temp.x, temp.y, -0.9f), Quaternion.identity);
				} else if( temp.next.y == temp.y &&  temp.prev.y < temp.y && temp.next.x < temp.x )
				{
					prevSankePart = Instantiate(snakeTurnDownRight, new Vector3(temp.x, temp.y, -0.9f), Quaternion.identity);
				} 
				
				
				else if( temp.next.x == temp.x && temp.prev.x > temp.x && temp.next.y > temp.y )
				{
					prevSankePart = Instantiate(snakeTurnUpLeft, new Vector3(temp.x, temp.y, -0.9f), Quaternion.identity);
				} else if( temp.next.x == temp.x && temp.prev.x > temp.x && temp.next.y < temp.y )
				{
					prevSankePart = Instantiate(snakeTurnDownLeft, new Vector3(temp.x, temp.y, -0.9f), Quaternion.identity);
				} else if( temp.next.x == temp.x && temp.prev.x < temp.x && temp.next.y > temp.y )
				{
					prevSankePart = Instantiate(snakeTurnUpRight, new Vector3(temp.x, temp.y, -0.9f), Quaternion.identity);
				} else if( temp.next.x == temp.x && temp.prev.x < temp.x && temp.next.y < temp.y )
				{
					prevSankePart = Instantiate(snakeTurnDownRight, new Vector3(temp.x, temp.y, -0.9f), Quaternion.identity);
				} 
				
				
				//prevSankePart = Instantiate(SnakePart, new Vector3(temp.x, temp.y, -0.5f), Quaternion.identity);
			}
			
			
			//prevSankePart = Instantiate(SnakePart, new Vector3(temp.x, temp.y, 0), Quaternion.identity);
			parts.AddLast(prevSankePart);
			
			counter++;
		}
		
		if( appleCreated == false )
		{
			prevApple= Instantiate(apple, new Vector3(appleX, appleY, 0), Quaternion.identity);
			appleCreated = true;
		}
		
	}
	
	
}
