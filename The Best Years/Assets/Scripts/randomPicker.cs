/*public class randomPicker {
    public List<int> first = new List<int>();
    public List<int> second = new List<int>();
    Random rnd = new Random();

    for(int i = 0; i<10; i++){
        first.Add(i); 
    }
    Console.WriteLine(first);
    Console.WriteLine(second);

    public List<int> pickRandomly(List<int> one, List<int> two){
        int selector = 0;
        if(one.Length > 1){
            selector = rnd.Next(0, (one.Length - 1));
            temp = one[selector];
            two.Add(temp)
            one.RemoveAt(selector);
            return one, two;
        }
        else{
            temp = one[0];
            two.Add(temp)
            one.RemoveAt(selector);
            one, two = isEmpty(one,two);
            return one, two;
        }
    }

    public List<int> isEmpty(List<int> one, List<int> two){
        if(one.Length == 0){
            one = two;
            for(int i = 0; i < two.length - 1; i++){
                two[i] = 0;
            }
        }
        return one, two;
    }
}*/