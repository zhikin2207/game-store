using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using GameStore.Domain.Entities;
using GameStore.Domain.Entities.Components;
using Publisher = GameStore.Domain.Entities.Publisher;

namespace GameStore.Domain.DbInitializers
{
	public class GameStoreDbAutomatedInitializer : CreateDatabaseIfNotExists<GameStoreContext>
	{
		private const int GenresCount = 10;
		private const int PlatformsCount = 5;
		private const int PublishersCount = 10;
		private const int GamesCount = 10;

		private const string GameKeyTemplate = "game-key-{0}";
		private const int MaxGenresPerGame = 4;
		private const int MaxPlatformsPerGame = 4;
		private const int MaxCommentsPerGame = 5;
		private const int MaxViewsPerGame = 1;

		#region WORDS

		private readonly string[] _words =
		{
			"Abandon", "Association", "Bore", "Chemist", "Criminal", "Ease", "Abandoned", "Atmosphere", "Bored",
			"Chemistry", "Crop", "East", "Absolute", "Atom", "Boring", "Cheque", "Cross", "Eastern", "Abuse", "Attached",
			"Bottle", "Chest", "Crowd", "Eat", "Academic", "Attack", "Bowl", "Chew", "Crowded", "Economic", "Access",
			"Attention", "Box", "Chicken", "Crown", "Economy", "Accident", "Attorney", "Boy", "Chief", "Cry", "Edge",
			"Accommodation", "Attraction", "Boyfriend", "Child", "Cup", "Editor", "Account", "Attractive", "Brain",
			"Chin", "Cupboard", "Educate", "Accuse", "Audience", "Branch", "Chip", "Cure", "Educated", "Achieve", "Aunt",
			"Brave", "Chocolate", "Curl", "Education", "Achievement", "Author", "Bread", "Choice", "Curly", "Efficient",
			"Acid", "Automatic", "Break", "Choose", "Curtain", "Effort", "Acquire", "Autumn", "Breakfast", "Chop",
			"Curved", "Egg", "Actor", "Available", "Breath", "Church", "Customer", "Elbow", "Actress", "Average",
			"Breathing", "Cigarette", "Cut", "Elderly", "Add", "Awake", "Brick", "Cinema", "Cycling", "Election",
			"Addition", "Award", "Bridge", "Circle", "Dad", "Electrical", "Adjust", "Awful", "Bright", "Citizen",
			"Damage", "Electricity", "Adopt", "Baby", "Brilliant", "Claim", "Dance", "Electronic", "Adult", "Back",
			"Bring", "Clap", "Dancer", "Elegant", "Advertisement", "Back", "Broadcast", "Class", "Dancing", "Element",
			"Advertising", "Background", "Broken", "Class", "Danger", "Elephant", "Afraid", "Bacteria", "Brother",
			"Classroom", "Dangerous", "Elevator", "Aged", "Bad", "Brown", "Clean", "Date", "Email", "Agreement",
			"Bad-tempered", "Brush", "Clerk", "Daughter", "Embarrassing", "Aid", "Badly", "Brush", "Client", "Dead",
			"Embarrassment", "Aircraft", "Bag", "Brush", "III", "Climb", "Deaf", "Emerge", "Airport", "Baggage",
			"Bubble", "Climbing", "Death", "Emergency", "Alarm", "Bake", "Buggy", "Clock", "Debt", "Emotion", "Alarmed",
			"Balance", "Build", "Closed", "Decorate", "Emotional", "Alarming", "Ball", "Building", "Closet",
			"Decoration", "Emotionally", "Alcohol", "Ball", "Bullet", "Cloth", "Defence", "Emphasis", "Alive", "Ban",
			"Bunch", "Clothes", "Defend", "Emphasize", "All", "right", "Band", "Burnt", "Clothing", "Delight", "Empire",
			"Allied", "Bandage", "Bury", "Cloud", "Delighted", "Employ", "Ally", "Bank", "Bus", "Club", "Delivery",
			"Employee", "Alone", "Bank", "Bush", "Coach", "Dentist", "Employer", "Along", "Bar", "Businessman", "Coal",
			"Deposit", "Employment", "Alongside", "Bar", "Businesswoman", "Coast", "Desert", "Empty", "Aloud", "Bargain",
			"Busy", "Coat", "Deserted", "Encounter", "Alphabet", "Barrier", "Butter", "Coffee", "Desk", "Encounter",
			"Alphabetical", "Base", "Button", "Coin", "Destruction", "Encourage", "Alphabetically", "Bath", "Buy",
			"Cold", "Device", "Encouragement", "Alternative", "Bath", "Buyer", "Coldly", "Device", "End",
			"Alternatively", "Bathroom", "Cabinet", "Collapse", "Diagram", "Enemy", "Ambulance", "Battery", "Cabinet",
			"Colleague", "Diamond", "Energy", "Amuse", "Battery", "Cable", "Collect", "Diary", "Engaged", "Analyse",
			"Battle", "Cable", "Collection", "Dictionary", "Engaged", "Analysis", "Bay", "Cake", "Colour", "Die",
			"Engine", "Ancient", "Be", "sick", "Calculate", "Coloured", "Diet", "Engine", "Anger", "Beach", "Call",
			"Column", "Dig", "Engineer", "Angle", "Beak", "Camera", "Come", "Dinner", "Engineering", "Angry", "Bear",
			"Camp", "Comedy", "Dirt", "Enjoy", "Animal", "Bear", "Campaign", "Comfort", "Dirty", "Enjoyment",
			"Anniversary", "Beard", "Camping", "Command", "Disabled", "Enormous", "Announce", "Beat", "Cancel",
			"Compete", "Disapproval", "Enquiry", "Annoyed", "Beautiful", "Cancer", "Competition", "Disapprove", "Ensure",
			"Answer", "Beauty", "Candidate", "Computer", "Disaster", "Enter", "Anxiety", "Bed", "Candy", "Concert",
			"Disc", "Entertaining", "Anxious", "Bedroom", "Cap", "Conference", "Disease", "Entertainment", "Apart",
			"Beef", "Capital", "Conflict", "Dish", "Enthusiastic", "Apartament", "Beer", "Captain", "Confuse", "Display",
			"Entire", "Apologize", "Belief", "Capture", "Confused", "Divide", "Entitle", "Appear", "Bell", "Car",
			"Confusion", "Division", "Entrance", "Appearance", "Belt", "Card", "Congratulations", "Do", "Entry",
			"Appearance", "Bend", "Carpet", "Connection", "Doctor", "Envelope", "Apple", "Beneath", "Carrot",
			"Construct", "Dog", "Environment", "Application", "Between", "Carrot", "Construction", "Dollar",
			"Environmental", "Appointment", "Bicycle", "Carry", "Container", "Door", "Equal", "Approval", "Bid", "Cash",
			"Continent", "Dot", "Equipment", "Approximate", "Big", "Cast", "Contrast", "Down", "Equivalent", "April",
			"Bill", "Castle", "Convention", "Downward", "Error", "Area", "Bin", "Cat", "Conversation", "Dozen", "Escape",
			"Argue", "Biology", "Catch", "Cook", "Draft", "Essay", "Argument", "Bird", "CD", "Cooker", "Drama",
			"Essential", "Arise", "Birth", "Ceiling", "Cookie", "Draw", "Estate", "Arm", "Birthday", "Celebrate",
			"Cooking", "Drawer", "Estimate", "Armed", "Biscuit", "Celebration", "Core", "Drawer", "Euro", "Army",
			"Bitter", "Cell", "Corner", "Dream", "Even", "Around", "Bitterly", "Cellphone", "Cost", "Dress", "Even",
			"Arrange", "Black", "Cent", "Cottage", "Dressed", "Evening", "Arrangement", "Blade", "Centimetre", "Cotton",
			"Drink", "Event", "Arrest", "Blank", "Ceremony", "Cough", "Drive", "Everyone", "Arrival", "Blame",
			"Certificate", "Coughing", "Driver", "Everywhere", "Arrive", "Blind", "Chain", "Count", "Driving",
			"Evidence", "Arrow", "Blind", "Chair", "Counter", "Drop", "Evil", "Art", "Block", "Chamber", "Countryside",
			"Drug", "Exact", "Article", "Blonde", "Change", "Couple", "Drugstore", "Exactly", "Artist", "Blood",
			"Channel", "Court", "Drum", "Exaggeration", "Artistic", "Blue", "Charity", "Cover", "Drunk", "Exaggerate",
			"Artistic", "Board", "Chart", "Cover", "Dry", "Exaggeration", "Ashamed", "Boat", "Chase", "Cow", "Dull",
			"Exam", "Ask", "Boil", "Chat", "Cracked", "Dump", "Examination", "Asleep", "Bomb", "Cheat", "Crash", "Dust",
			"Examine", "Aspect", "Bone", "Check", "Crash", "Dying", "Example", "Assist", "Book", "Check", "Crawfish",
			"Ear", "Excellent", "Assistance", "Boot", "Cheek", "Cream", "Early", "Except", "Assistant", "Border",
			"Cheese", "Credit", "Card", "Earn", "Exception", "Associate", "Border", "Chemical", "Crime", "Earth",
			"Exchange", "Excite", "Firm", "Glue", "Hook", "Insurance", "Learn", "Excited", "First", "Go", "Hope",
			"Intellegence", "Leave", "Excitement", "Fish", "Go", "Bad", "Horizontal", "Intellegent", "Lecture",
			"Exciting", "Fishing", "Go", "Down", "Horn", "Interested", "Left", "Exclude", "Fit", "Go", "Horror",
			"Interesting", "Leg", "Excluding", "Fix", "Goal", "Horse", "Interior", "Legal", "Excuse", "Fixed", "God",
			"Hospital", "Internal", "Lemon", "Executive", "Flag", "Gold", "Hot", "International", "Lend", "Exercise",
			"Flame", "Good", "Hotel", "Internet", "Length", "Exercise", "Flash", "Goods", "Hour", "Interpret", "Less",
			"Exhibit", "Flat", "Govern", "House", "Interpretation", "Lesson", "Exhibition", "Flavour", "Government",
			"Household", "Interruption", "Letter", "Exit", "Flesh", "Governor", "Household", "Interval", "Letter",
			"Expand", "Flight", "Grab", "Housing", "Interview", "Level", "Expect", "Float", "Grade", "Huge", "Introduce",
			"Library", "Expectation", "Flood", "Grain", "Human", "Introduce", "Licence", "Expectation", "Floor", "Gram",
			"Humorous", "Introduction", "Lid", "Expected", "Flour", "Grandchild", "Humour", "Introduction", "Lie",
			"Expense", "Flow", "Granddaughter", "Hungry", "Invent", "Lift", "Expensive", "Flower", "Grandfather", "Hunt",
			"Invention", "Light", "Experienced", "Flu", "Grandmother", "Hunting", "Invest", "Like", "Experiment", "Fly",
			"Grandparents", "Hurry", "Investigate", "Limit", "Expert", "Fly", "Grandson", "Husband", "Investigation",
			"Line", "Explain", "Flying", "Grant", "Ice", "Investment", "Line", "Explanation", "Focus", "Grass", "Ice",
			"cream", "Invitation", "Link", "Explode", "Fold", "Grave", "Idea", "Invite", "Lip", "Explore", "Fold",
			"Great", "Ideal", "Iron", "Liquid", "Explosion", "Folding", "Green", "Identify", "Irritate", "List",
			"Export", "Food", "Grey", "Identify", "Irritated", "Listen", "Expose", "Foot", "Groceries", "Ignore",
			"Irritating", "Literature", "Extend", "Football", "Grocery", "Ill", "Island", "Litre", "Extension", "Force",
			"Ground", "Illegal", "Issue", "Little", "Extra", "Forecast", "Group", "Illegally", "Item", "Living",
			"Extraordinary", "Forest", "Grow", "Illness", "Jacket", "Load", "Extreme", "Forgive", "Grow", "Illustrate",
			"Jam", "Loan", "Eye", "Fork", "Growth", "Image", "Jealous", "Location", "Face", "Formula", "Guard",
			"Imagination", "Jeans", "Lock", "Facility", "Fortune", "Guest", "Imagine", "Jelly", "Logic", "Factory",
			"Forward", "Guide", "Immortal", "Jewelery", "Lonely", "Fail", "Foundation", "Guilty", "Impact", "Job",
			"Long", "Failure", "Frame", "Gun", "Impatient", "Join", "Look", "after", "Faint", "Freeze", "Guy",
			"Impatiently", "Joint", "Look", "at", "Faintly", "Friday", "Habit", "Implication", "Joint", "Look", "for",
			"Fair", "Fridge", "Hair", "Import", "Jointly", "Look", "forward", "Fairly", "Friend", "Hairdresser",
			"Important", "Joke", "Lord", "Fairly", "Friendly", "Half", "Impress", "Journalist", "Lorry", "Faith",
			"Friendship", "Hall", "Impressed", "Journey", "Lose", "Faithful", "Frighten", "Hammer", "Impresson", "Joy",
			"Loss", "Fall", "Frightened", "Hand", "Impressive", "Judge", "Lost", "Fall", "over", "Frightening", "Handle",
			"Improve", "Judgement", "Loud", "False", "Front", "Hang", "Improvement", "Juice", "Loudly", "Fame", "Frozen",
			"Happy", "Jump", "Love", "Family", "Fruit", "Happyness", "a", "hurry", "Junior", "Lovely", "Famous", "Fry",
			"Hard", "exchange", "for", "Justice", "Lover", "Fan", "Fuel", "Harm", "front", "Key", "Low", "Fancy", "Full",
			"Harmful", "honour", "Keyboard", "Low", "Far", "Fun", "Harmless", "memory", "of", "Kick", "Luck", "Farm",
			"Function", "Hat", "public", "Kid", "Lucky", "Farmer", "Fund", "Hate", "Inability", "Kid", "Luggage",
			"Farming", "Funeral", "Hatred", "Inch", "Kill", "Lump", "Fashion", "Funny", "He", "Incident", "Kindness",
			"Lunch", "Fashionable", "Fur", "Head", "Income", "King", "Lung", "Fast", "Furniture", "Headache", "Increase",
			"Kiss", "Machine", "Fasten", "Gain", "Heal", "Independent", "Kitchen", "Machinery", "Fat", "Gain", "Health",
			"Index", "Knee", "Mad", "Father", "Gamble", "Healthy", "Indicate", "Knife", "Magazine", "Faucet", "Gambling",
			"Hear", "Indication", "Knight", "Magazine", "Fault", "Game", "Hearing", "Indirect", "Knit", "Magic",
			"Favour", "Gap", "Heaven", "Individual", "Knitted", "Mail", "Favorite", "Garage", "Heavily", "Indoor",
			"Knitting", "Maintain", "Fear", "Garbage", "Heavy", "Indoors", "Knock", "Major", "Feather", "Garden", "Heel",
			"Industrial", "Knot", "Majority", "Federal", "Gas", "Height", "Industry", "Lab", "Make", "Fee", "Gasoline",
			"Hell", "Infect", "Laboratory", "Make", "friends", "Feed", "Gate", "Help", "Infection", "Labour", "Make",
			"fun", "of", "Feel", "Gather", "Helpful", "Infectious", "Lady", "Make-up", "Feel", "Sick", "Gear", "Here",
			"Influence", "Lake", "Make-up", "Feeling", "Generate", "Hero", "Inform", "Lamp", "Male", "Fellow",
			"Generation", "Hesitate", "Informal", "Land", "Mall", "Female", "Generous", "Hide", "Information",
			"Landscape", "Man", "Fence", "Gentle", "Hide", "Ingridients", "Lane", "Manage", "Festival", "Gentleman",
			"High", "Initial", "Language", "Management", "Fetch", "Geography", "Highlight", "Injure", "Large", "Manager",
			"Fever", "Get", "off", "Highway", "Injured", "Last", "Manufacture", "Field", "Get", "on", "Hill", "Injured",
			"Late", "Manufacturer", "Fight", "Giant", "Hip", "Injury", "Latter", "Manufacturing", "Fighting", "Gift",
			"Hire", "Ink", "Laugh", "Many", "Figure", "Girl", "Historical", "Innocent", "Launch", "Map", "File",
			"Girlfriend", "History", "Inquiry", "Law", "March", "File", "Give", "Birth", "Hit", "Insect", "Lawyer",
			"Mark", "Fill", "Give", "Hobby", "Insert", "Lay", "Market", "Film", "Give", "Hold", "Inside", "Layer",
			"Marketing", "Final", "Give", "Hole", "Install", "Lazy", "Marriage", "Finance", "Glad", "Holiday",
			"Instance", "Lead", "Married", "Financial", "Glass", "Hollow", "Institute", "Leader", "Marry", "Find",
			"Glasses", "Holy", "Institution", "Leading", "Mask", "Finger", "Global", "Home", "Instruction", "Leaf",
			"Mass", "Finish", "Glove", "Homework", "Instrument", "League", "Massive", "Fire", "Glue", "Honour", "Insult",
			"Lean", "Master", "Match", "Nerve", "Pass", "Pot", "Rail", "Road", "Matching", "Nervous", "Pass", "Potato",
			"Railway", "Rob", "Mate", "Nervously", "Passage", "Potential", "Rain", "Rock", "Mate", "Nest", "Passenger",
			"Pound", "Raise", "Role", "Material", "Net", "Passport", "Pound", "Range", "Roll", "Mathematics", "Network",
			"Path", "Pound", "III", "Rank", "Romantic", "Maximum", "New", "Patience", "Pour", "Rapid", "Roof", "Mayor",
			"News", "Patient", "Powder", "Rate", "Room", "Meal", "Newspaper", "Pattern", "Power", "Rate", "Root", "Meal",
			"Nice", "Pause", "Powerful", "Raw", "Rope", "Mean", "Night", "Pay", "Praise", "Reach", "Rough", "Means",
			"Noise", "Payment", "Prayer", "Read", "Roughly", "Measurement", "Noisily", "Peace", "Predict", "Reader",
			"Round", "Meat", "Noisy", "Peaceful", "Preference", "Reading", "Rounded", "Media", "Nonsense", "Peak",
			"Pregnant", "Real", "Route", "Media", "Normal", "Peak", "Premises", "Reality", "Royal", "Medical", "North",
			"Pen", "Preparation", "Reason", "Rub", "Medicine", "Nose", "Pencil", "Prepare", "Reasonable", "Rubber",
			"Meet", "Note", "Penny", "Prepared", "Recall", "Rubbish", "Meeting", "Note", "Pension", "Present", "Receipt",
			"Rude", "Melt", "Notice", "People", "Presentation", "Receive", "Rudely", "Membership", "Novel", "Pepper",
			"Preserve", "Reception", "Ruin", "Memory", "Nuclear", "Per", "cent", "Preserve", "Reckon", "Ruined",
			"Mental", "Number", "Perform", "President", "Record", "Rule", "Mention", "Nurse", "Performance", "Press",
			"Recording", "Ruler", "Menu", "Nut", "Performer", "Pressure", "Red", "Rumour", "Mere", "Object", "Period",
			"Pretend", "Reduction", "Run", "Mess", "Objective", "Person", "Pretty", "Reflect", "Runner", "Message",
			"Observation", "Personality", "Prevent", "Refrigerator", "Running", "Metal", "Occupied", "Pester", "Price",
			"Refuse", "Rural", "Metre", "Occupy", "Pet", "Pride", "Refuse", "Rush", "Middle", "Ocean", "Petrol",
			"Priest", "Region", "Rush", "Midnight", "O'clock", "Phase", "Prince", "Register", "Sack", "Mild", "Odd",
			"Philosophy", "Princess", "Regret", "Sad", "Mild", "Oddly", "Phone", "Print", "Regulation", "Sadly", "Mile",
			"Off", "Photocopy", "Printer", "Reject", "Sadness", "Military", "Offend", "Photograph", "Printing", "Relate",
			"Safe", "Milk", "Offensive", "Photographer", "Priority", "Relation", "Safely", "Milligram", "Offensive",
			"Photography", "Prison", "Relationship", "Safety", "Millimetre", "Offer", "Phrase", "Prisoner", "Relative",
			"Sail", "Mind", "Office", "Physics", "Prize", "Relax", "Sail", "Mineral", "Officer", "Piano", "Problem",
			"Relaxed", "Sailing", "Minimum", "Official", "Pick", "Proceed", "Relaxing", "Sailor", "Minister",
			"Officially", "Pick", "Process", "Relief", "Salad", "Ministry", "Old-fashioner", "Picture", "Produce",
			"Religion", "Salary", "Minute", "Onion", "Piece", "Product", "Religious", "Sale", "Mirror", "Onto", "Pig",
			"Production", "Remain", "Salt", "Miss", "Open", "Pile", "Profession", "Remains", "Salty", "Mistake",
			"Opening", "Pill", "Professional", "Remember", "Sample", "Mix", "Opinion", "Pilot", "Professor", "Remind",
			"Sand", "Mixed", "Opponent", "Pilot", "Profit", "Removal", "Satisfaction", "Mixture", "Opportunity", "Pin",
			"Program", "Remove", "Satisfied", "Mobile", "Opposing", "Pink", "Progress", "Rent", "Satisfy", "Mobile",
			"phone", "Opposite", "Pipe", "Project", "Rent", "Satisfying", "Model", "Opposition", "Pitch", "Promise",
			"Repair", "Sauce", "Modern", "Option", "Place", "Promote", "Repeat", "Save", "Mom", "Or", "Place",
			"Promotion", "Replace", "Saving", "Money", "Orange", "Plain", "Pronounce", "Reply", "Say", "Monitor",
			"Ordinal", "Numbers", "Plan", "Pronunciation", "Report", "Scale", "Monitor", "Organ", "Plane", "Proof",
			"Rescue", "Scare", "Month", "Organization", "Plane", "Property", "Research", "Scared", "Moon", "Organize",
			"Planet", "Proportion", "Reservation", "Scene", "Morning", "Origin", "Planning", "Proposal", "Resist",
			"Schedule", "Mother", "Original", "Plant", "Propose", "Resolve", "Scheme", "Motion", "Out", "Plastic",
			"Prospect", "Resort", "School", "Motor", "Outdoor", "Plate", "Prospect", "Resource", "Science", "Motorcycle",
			"Outdoors", "Plate", "Protect", "Respect", "Scientific", "Mount", "Outline", "Platform", "Protection",
			"Respond", "Scientist", "Mount", "Output", "Play", "Protest", "Response", "Scissors", "Mountain", "Outside",
			"Player", "Proud", "Responsibility", "Score", "Mouse", "Outstanding", "Pleased", "Prove", "Rest", "Scratch",
			"Mouth", "Oven", "Pleasure", "Provide", "Restaurant", "Scream", "Move", "Over", "Plenty", "Pub", "Restore",
			"Screen", "Movie", "Overall", "Plot", "Publcation", "Restrict", "Screw", "Movie", "Theater", "Pace", "Plug",
			"Publicity", "Restricted", "Sea", "Mud", "Pack", "Plus", "Publish", "Restriction", "Seal", "Mum", "Package",
			"Pocket", "Publishing", "Result", "Search", "Murder", "Packaging", "Poem", "Pull", "Retain", "Season",
			"Muscle", "Packet", "Poetry", "Punch", "Retire", "Seat", "Museum", "Page", "Point", "Punish", "Retired",
			"Secondary", "Music", "Pain", "Pointed", "Punishment", "Retirement", "Secret", "Musical", "Painful",
			"Poison", "Pupil", "Return", "Secret", "Musician", "Paint", "Poisonous", "Purchase", "Review", "Secretary",
			"Mysterious", "Painter", "Pole", "Pure", "Revise", "Secretly", "Mystery", "Painting", "Police", "Purple",
			"Revision", "Sector", "Nail", "Pair", "Policy", "Purpose", "Revolution", "Security", "Naked", "Palace",
			"Polish", "Pursue", "Reward", "See", "Narrow", "Pale", "Polite", "Push", "Rhythm", "Seed", "National", "Pan",
			"Political", "Put", "Rice", "Seek", "Navy", "Pants", "Politician", "Quality", "Rich", "Selection", "Near",
			"Paper", "Pollution", "Quarter", "Ride", "Sell", "Nearby", "Parallel", "Pool", "Queen", "Rider", "Send",
			"Neat", "Parallel", "Poor", "Question", "Ridiculous", "Senior", "Neat", "Parent", "Pop", "Quiet", "Riding",
			"Sentence", "Neatly", "Park", "Popular", "Quietly", "Ring", "Separate", "Neck", "Parliament", "Population",
			"Quit", "Rise", "Serious", "Needle", "Part", "Port", "Quote", "Rising", "Servant", "Negative", "Partner",
			"Pose", "Race", "Risk", "Serve", "Neighbour", "Partnership", "Post", "Racing", "Rival", "Service",
			"Neighbourhood", "Party", "Post", "office", "Radio", "River", "Set", "fire", "Set", "Sock", "Stripe",
			"Tension", "Trip", "Vocabulary", "Sew", "Software", "Striped", "Tent", "Tropical", "Volume", "Sewing",
			"Soil", "Stroke", "Test", "Trousers", "Volume", "Sex", "Soil", "Strong", "Thank", "Truck", "Vote", "Shade",
			"Soldier", "Struggle", "Theatre", "Tube", "Wage", "Shadow", "Son", "Student", "Theory", "Tune", "Waist",
			"Shake", "Song", "Studio", "Thick", "Tunnel", "Wait", "Shallow", "Sore", "Study", "Thickness", "Turn",
			"Waiter", "Shame", "Sore", "Stuff", "Thief", "TV", "Waiter", "Shape", "Sorry", "Stupid", "Thin", "Twin",
			"Walk", "Share", "Soul", "Style", "Think", "Twist", "Walking", "Sharp", "Sound", "Succeed", "Thinking",
			"Twisted", "Wall", "Shave", "Soup", "Success", "Thirsty", "Tyre", "Wallet", "Sheep", "Sour", "Successful",
			"Thought", "Ugly", "War", "Sheet", "Source", "Suck", "Thread", "Umbrella", "Warm", "Shelf", "South",
			"Suffer", "Threat", "Unable", "Warmth", "Shell", "Speak", "Suffering", "Threaten", "Uncertain", "Warn",
			"Shelter", "Speaker", "Sugar", "Threatening", "Uncle", "Warning", "Shift", "Speech", "Suit", "Throat",
			"Under", "Wash", "Shine", "Speed", "Suitcase", "Throw", "Underground", "Washing", "Shiny", "Spell", "Sum",
			"Thumb", "Underneath", "Watch", "Ship", "Spelling", "Summer", "Thumb", "Understand", "Watch", "Shirt",
			"Spice", "Sun", "Tick", "Understanding", "Water", "Shock", "Spicy", "Sun", "Ticket", "Underwater", "Wave",
			"Shocked", "Spider", "Supermarket", "Tidy", "Underwear", "Way", "Shocking", "Spider", "Surface", "Tie",
			"Unemployed", "Weakness", "Shoe", "Spin", "Surface", "Tie", "Unexpected", "Wealth", "Shoot", "Spin",
			"Surprise", "Tight", "Unfair", "Weapon", "Shooting", "Spirit", "Survey", "Till", "Unfortunate", "Weather",
			"Shop", "Spiritual", "Survey", "Time", "Unfriendly", "Web", "Shopping", "Split", "Suspect", "Timetable",
			"Unhappiness", "Wedding", "Short", "Spoil", "Suspicious", "Tin", "Unhappy", "Weekend", "Shoulder", "Spoon",
			"Swallow", "Tin", "Uniform", "Weigh", "Shout", "Sport", "Swallow", "Tiny", "Union", "Weight", "Show", "Spot",
			"Swallow", "III", "Tip", "Unique", "Welcome", "Shower", "Spray", "Swear", "Tire", "Unit", "Well", "Shower",
			"Spray", "Swearing", "Tire", "Unite", "West", "Shy", "Spread", "Sweat", "Tired", "United", "Western", "Sick",
			"Spread", "Sweater", "Tiring", "Universe", "Wet", "Sight", "Spring", "Sweater", "Title", "University",
			"Wheel", "Sign", "Square", "Sweep", "Toe", "Unkind", "Whisper", "Signal", "Squeeze", "Sweet", "Toilet",
			"Unknown", "Whistle", "Signature", "Staff", "Swell", "Toilet", "Unlike", "White", "Silence", "Stage",
			"Swelling", "Tomato", "Unload", "Width", "Silent", "Stair", "Swim", "Ton", "Unlucky", "Wife", "Silk",
			"Stamp", "Swimming", "Tone", "Untidy", "Wild", "Silly", "Standard", "Swimming", "Pool", "Tongue", "Unusual",
			"Win", "Silver", "Star", "Swing", "Tool", "Wind", "Sing", "Stare", "Switch", "Tooth", "Upon", "Wind",
			"Singer", "Start", "Switch", "Top", "Upper", "Window", "Singing", "Station", "Swollen", "Torment",
			"Upside-down", "Wine", "Single", "Station", "Symbol", "Total", "Upstairs", "Wing", "Sink", "Statue", "Table",
			"Touch", "Upward", "Winner", "Sink", "Steal", "Tablet", "Tour", "Upwards", "Winning", "Sister", "Steam",
			"Tackle", "Tourist", "Urban", "Winter", "Sit", "Steel", "Tackle", "Towel", "Urgent", "Wire", "Size", "Steer",
			"Tail", "Tower", "Used", "Witness", "Skilful", "Step", "Take", "part", "Town", "Usual", "Woman", "Skill",
			"Stick", "Talk", "Toy", "Vacation", "Wood", "Skilled", "Sticky", "Tall", "Track", "Valley", "Wooden",
			"Skirt", "Sting", "Tank", "Trade", "Valuable", "Wool", "Sky", "Stomach", "Tap", "Trading", "Van", "Worker",
			"Sleep", "Stone", "Tap", "Tradition", "Vanish", "Working", "Sleeve", "Stop", "Tape", "Traditional",
			"Variety", "World", "Slice", "Storm", "Target", "Traffic", "Various", "Worship", "Slide", "Story", "Taste",
			"Train", "Vast", "Wound", "Slight", "Stove", "Taxi", "Train", "Vegetable", "Wrap", "Slope", "Straight",
			"Tea", "Transfer", "Vehicle", "Wrapping", "Slow", "Strain", "Teach", "Transform", "Vertical", "Wrist",
			"Slowly", "Stranger", "Teacher", "Translate", "Victim", "Write", "Small", "Strategy", "Teaching",
			"Translation", "Victory", "Writer", "Smash", "Stream", "Team", "Transparent", "Video", "Writing", "Smell",
			"Street", "Tear", "Transport", "View", "Wrong", "Smile", "Strength", "Tear", "Trap", "Village", "Yard",
			"Smoke", "Stress", "Technique", "Travel", "Violence", "Yawn", "Smoking", "Stressed", "Telephone",
			"Traveller", "Violent", "Year", "Snake", "Stretch", "Telephone", "Tree", "Vision", "Yellow", "Snow",
			"Strike", "Television", "Triangle", "Visit", "Young", "Soap", "String", "Temperature", "Trick", "Visitor"
		};

		#endregion

		protected override void Seed(GameStoreContext context)
		{
			InitManuallyGenres(context);

			InitPlatforms(context);
			InitPublishers(context);
			InitRoles(context);
			InitUsers(context);
			InitGames(context);

			InitPublishserUser(context);

			AddConstraints(context);

			base.Seed(context);
		}

		private static void AddConstraints(GameStoreContext context)
		{
			context.Database.ExecuteSqlCommand("ALTER TABLE \"PlatformTypes\" ADD UNIQUE(\"Type\")");
			context.Database.ExecuteSqlCommand("ALTER TABLE \"Publishers\" ADD UNIQUE(\"CompanyName\")");
			context.Database.ExecuteSqlCommand("ALTER TABLE \"Genres\" ADD UNIQUE(\"Name\")");
			context.Database.ExecuteSqlCommand("ALTER TABLE \"Games\" ADD CONSTRAINT Game_Publisher FOREIGN KEY (PublisherId) REFERENCES dbo.Publishers(PublisherId) ON UPDATE NO ACTION ON DELETE SET NULL");
			context.Database.ExecuteSqlCommand("ALTER TABLE \"Comments\" ADD CONSTRAINT Comment_User FOREIGN KEY (UserGuid) REFERENCES dbo.Users(UserGuid) ON UPDATE NO ACTION ON DELETE SET NULL");
		}

		#region Init database entities

		private void InitGenres(GameStoreContext context)
		{
			var random = new Random();
			for (int i = 0; i < GenresCount; i++)
			{
				context.Genres.Add(GetRandomGenre(random));
			}

			context.SaveChanges();
		}

		private void InitManuallyGenres(GameStoreContext context)
		{
			context.Genres.Add(new Genre { Name = "Strategy", ParentGenreId = null });
			context.Genres.Add(new Genre { Name = "RPG", ParentGenreId = null });
			context.Genres.Add(new Genre { Name = "Sports", ParentGenreId = null });
			context.Genres.Add(new Genre { Name = "Races", ParentGenreId = null });
			context.Genres.Add(new Genre { Name = "Action", ParentGenreId = null });
			context.Genres.Add(new Genre { Name = "Adventure", ParentGenreId = null });
			context.Genres.Add(new Genre { Name = "Puzzle&Skill", ParentGenreId = null });

			// "Strategy" nested genres
			context.Genres.Add(new Genre { Name = "RTS", ParentGenreId = 1 });
			context.Genres.Add(new Genre { Name = "TBS", ParentGenreId = 1 });

			// "Races" nested genres
			context.Genres.Add(new Genre { Name = "Rally", ParentGenreId = 4 });
			context.Genres.Add(new Genre { Name = "Arcade", ParentGenreId = 4 });
			context.Genres.Add(new Genre { Name = "Formula", ParentGenreId = 4 });
			context.Genres.Add(new Genre { Name = "Off-road", ParentGenreId = 4 });

			// "Action" nested genres
			context.Genres.Add(new Genre { Name = "FPS", ParentGenreId = 5 });
			context.Genres.Add(new Genre { Name = "TPS", ParentGenreId = 5 });
			context.Genres.Add(new Genre { Name = "Misc", ParentGenreId = 5 });
			context.SaveChanges();
		}

		private void InitPlatforms(GameStoreContext context)
		{
			var random = new Random();
			for (int i = 0; i < PlatformsCount; i++)
			{
				context.PlatformTypes.Add(GetRandomPlatform(random));
			}

			context.SaveChanges();
		}

		private void InitPublishers(GameStoreContext context)
		{
			var random = new Random();
			for (int i = 0; i < PublishersCount; i++)
			{
				context.Publishers.Add(GetRandomPublisher(random));
			}

			context.SaveChanges();
		}

		private void InitRoles(GameStoreContext context)
		{
			string[] roles = { "Administrator", "Manager", "Moderator", "User", "Publisher" };

			foreach (string role in roles)
			{
				context.Roles.Add(new Role
				{
					Name = role,
					IsSystem = true
				});
			}

			context.SaveChanges();
		}

		private void InitUsers(GameStoreContext context)
		{
			var admin = new User
			{
				UserGuid = Guid.NewGuid(),
				Email = "admin@mail.com",
				Password = HashPassword("admin@mail.com"),
				Name = "Administrator",
				Role = context.Roles.First(r => r.Name == "Administrator"),
				CreatedAt = DateTime.UtcNow
			};

			var manager = new User
			{
				UserGuid = Guid.NewGuid(),
				Email = "manager@mail.com",
				Password = HashPassword("manager@mail.com"),
				Name = "Manager",
				Role = context.Roles.First(r => r.Name == "Manager"),
				CreatedAt = DateTime.UtcNow
			};

			var moderator = new User
			{
				UserGuid = Guid.NewGuid(),
				Email = "moderator@mail.com",
				Password = HashPassword("moderator@mail.com"),
				Name = "Moderator",
				Role = context.Roles.First(r => r.Name == "Moderator"),
				CreatedAt = DateTime.UtcNow
			};

			var user = new User
			{
				UserGuid = Guid.NewGuid(),
				Email = "user@mail.com",
				Password = HashPassword("user@mail.com"),
				Name = "User",
				Role = context.Roles.First(r => r.Name == "User"),
				CreatedAt = DateTime.UtcNow
			};

			context.Users.Add(admin);
			context.Users.Add(manager);
			context.Users.Add(moderator);
			context.Users.Add(user);

			context.SaveChanges();
		}

		private void InitPublishserUser(GameStoreContext context)
		{
			var publisher = new Publisher
			{
				CompanyName = "Publisher"
			};

			var publisherUser = new User
			{
				UserGuid = Guid.NewGuid(),
				Email = "publisher@mail.com",
				Password = HashPassword("publisher@mail.com"),
				Name = publisher.CompanyName,
				Role = context.Roles.First(r => r.Name == "Publisher"),
				CreatedAt = DateTime.UtcNow,
				Publisher = publisher
			};

			context.Users.Add(publisherUser);
		}

		private void InitGames(GameStoreContext context)
		{
			var random = new Random();

			IEnumerable<int> publishersId = context.Publishers.Select(publisher => publisher.PublisherId).ToList();
			IEnumerable<int> genresId = context.Genres.Select(genre => genre.GenreId).ToList();
			IEnumerable<int> platformsId = context.PlatformTypes.Select(platform => platform.PlatformTypeId).ToList();

			for (int i = 0; i < GamesCount; i++)
			{
				Game game = GetRandomGame(string.Format(GameKeyTemplate, i), random);

				game.PublisherId = publishersId.ElementAt(random.Next(0, publishersId.Count()));

				InitGameGenres(context, game, genresId, random);
				InitGamePlatforms(context, game, platformsId, random);
				InitGameComments(context, game, random);

				WriteGameViewsHistory(context, game, random);
				WriteGameAddedHistory(context, game, random);

				context.Games.Add(game);
			}

			context.SaveChanges();
		}

		#endregion

		#region Init game entities

		private void InitGameGenres(GameStoreContext context, Game game, IEnumerable<int> genresId, Random random)
		{
			var currentGenresId = new List<int>(genresId);
			int gameGenresCount = random.Next(1, MaxGenresPerGame);

			for (int g = 0; g < gameGenresCount; g++)
			{
				int genreIndex = random.Next(0, currentGenresId.Count());
				game.Genres.Add(context.Genres.Find(currentGenresId[genreIndex]));
				currentGenresId.RemoveAt(genreIndex);
			}
		}

		private void InitGameComments(GameStoreContext context, Game game, Random random)
		{
			int gameCommentsCount = random.Next(1, MaxCommentsPerGame);

			var users = context.Users.ToList();

			Comment parent = null;
			for (int c = 0; c < gameCommentsCount; c++)
			{
				Comment comment = GetRandomComment(game.Key, random);
				comment.UserGuid = users[random.Next(0, context.Users.Count())].UserGuid;
				comment.Parent = parent;
				game.Comments.Add(comment);

				WriteGameCommentHistory(context, game);

				if (random.Next(10) % 2 == 0)
				{
					if (random.Next(15) % 3 == 0)
					{
						parent = null;
					}
					else
					{
						parent = game.Comments.ElementAt(random.Next(0, game.Comments.Count()));
					}
				}
			}
		}

		private void InitGamePlatforms(GameStoreContext context, Game game, IEnumerable<int> platformsId, Random random)
		{
			var currentPlatformsId = new List<int>(platformsId);
			int gamePlatformsCount = random.Next(1, MaxPlatformsPerGame);

			for (int p = 0; p < gamePlatformsCount; p++)
			{
				int platformIndex = random.Next(0, currentPlatformsId.Count());
				game.PlatformTypes.Add(context.PlatformTypes.Find(currentPlatformsId[platformIndex]));
				currentPlatformsId.RemoveAt(platformIndex);
			}
		}

		#endregion

		#region Write game operation history

		private void WriteGameCommentHistory(GameStoreContext context, Game game)
		{
			var operationHistory = new GameHistory
			{
				GameKey = game.Key,
				Date = DateTime.UtcNow,
				Type = OperationType.Comment,
			};

			context.OperationHistory.Add(operationHistory);
		}

		private void WriteGameViewsHistory(GameStoreContext context, Game game, Random random)
		{
			int currentGameViews = random.Next(1, MaxViewsPerGame);
			for (int v = 0; v < currentGameViews; v++)
			{
				var operationHistory = new GameHistory
				{
					GameKey = game.Key,
					Date = DateTime.UtcNow,
					Type = OperationType.View,
				};

				context.OperationHistory.Add(operationHistory);
			}
		}

		private void WriteGameAddedHistory(GameStoreContext context, Game game, Random random)
		{
			var operationHistory = new GameHistory
			{
				GameKey = game.Key,
				Date = GetRandomDate(2000, random),
				Type = OperationType.Add,
			};

			context.OperationHistory.Add(operationHistory);
		}

		#endregion

		#region Get random entity

		private Genre GetRandomGenre(Random random)
		{
			return new Genre
			{
				Name = GetRandomTitle(1, 2, random)
			};
		}

		private PlatformType GetRandomPlatform(Random random)
		{
			return new PlatformType
			{
				Type = GetRandomTitle(1, 2, random)
			};
		}

		private Publisher GetRandomPublisher(Random random)
		{
			var publisher = new Publisher
			{
				CompanyName = GetRandomTitle(1, 4, random),
				Description = GetRandomText(5, 15, random)
			};

			publisher.HomePage = string.Format("http://{0}.com", publisher.CompanyName.Replace(' ', '-').ToLower());

			return publisher;
		}

		private Comment GetRandomComment(string gameKey, Random random)
		{
			return new Comment
			{
				Name = GetRandomTitle(1, 3, random),
				GameKey = gameKey,
				Body = GetRandomText(4, 15, random),
				Date = GetRandomDate(2000, random)
			};
		}

		private Game GetRandomGame(string gameKey, Random random)
		{
			return new Game
			{
				Key = gameKey,
				Name = GetRandomTitle(3, 7, random),
				Description = GetRandomText(15, 30, random),
				Price = random.Next(100, 1000),
				UnitsInStock = (short)random.Next(0, 200),
				Discontinued = random.Next(10) % 2 == 0,
				DatePublished = GetRandomDate(2000, random),
				Genres = new List<Genre>(),
				PlatformTypes = new List<PlatformType>(),
				Comments = new List<Comment>()
			};
		}

		#endregion

		private string GetRandomTitle(int fromCountWords, int toCountWords, Random random)
		{
			int count = random.Next(fromCountWords, toCountWords);
			var title = new StringBuilder();

			title.Append(_words[random.Next(0, _words.Length)]);
			for (int i = 0; i < count - 1; i++)
			{
				title.Append(" ");
				title.Append(_words[random.Next(0, _words.Length)]);
			}

			return title.ToString();
		}

		private string GetRandomText(int fromCountWords, int toCountWords, Random random)
		{
			int count = random.Next(fromCountWords, toCountWords);
			var gameDescription = new StringBuilder();

			gameDescription.Append(_words[random.Next(0, _words.Length)]);
			for (int i = 0; i < count - 1; i++)
			{
				if (random.Next(100) % 10 == 0)
				{
					gameDescription.Append(". ");
					gameDescription.Append(_words[random.Next(0, _words.Length)]);
				}

				gameDescription.Append(" ");
				gameDescription.Append(_words[random.Next(0, _words.Length)].ToLower());
			}

			gameDescription.Append(".");

			return gameDescription.ToString();
		}

		private DateTime GetRandomDate(int fromYear, Random random)
		{
			int year = random.Next(fromYear, DateTime.UtcNow.Year);
			int month = random.Next(1, (year != DateTime.UtcNow.Year) ? 12 : DateTime.UtcNow.Month);
			int day = random.Next(
				1,
				(year != DateTime.UtcNow.Year || month != DateTime.UtcNow.Month) ? DateTime.DaysInMonth(year, month) : DateTime.UtcNow.Day);

			return new DateTime(year, month, day);
		}

		private string HashPassword(string password)
		{
			var bytes = Encoding.UTF8.GetBytes(password);

			using (MD5 md5Hash = MD5.Create())
			{
				var hash = md5Hash.ComputeHash(bytes);
				return ToHexString(hash);
			}
		}

		private string ToHexString(byte[] bytes)
		{
			var result = new StringBuilder();

			for (int i = 0; i < bytes.Length; i++)
			{
				result.Append(bytes[i].ToString("x2"));
			}

			return result.ToString();
		}
	}
}