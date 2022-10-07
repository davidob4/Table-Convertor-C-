using System; 
using System.Text.RegularExpressions;
class tabconv{
    static void Main(string[] args){
        table tabcon = new table();
        string ogFile = "", newGangsterFile = "";
        int picker = 0, otherPicker = 0;
        Boolean switcher = false;

        for(int i = 0; i < args.Length; i++){
            try{
                if(args[i].Contains("-i")) ogFile = args[i + 1]; 
                else if(args[i].Contains("-o")) newGangsterFile = args[i + 1];
                else if(args[i].Contains("-l")) Console.WriteLine("HTML, JSON, MD, CSV");
                else if(args[i].Contains("-V")) switcher = true;
                else if(args[i].Contains("-h")) Console.WriteLine("Help on the way!");
            }
            catch(Exception e){
                Console.WriteLine("Error!");
            }
        }

        if(switcher) tabcon.verboseMode();

        string[] ogFileEXTarr = ogFile.Split('.');
        string[] newGangsterFileEXTarr = newGangsterFile.Split('.');
        string ogFileEXT = ogFileEXTarr[1];
        string newGangsterFileEXT = newGangsterFileEXTarr[1];
        string raw = File.ReadAllText(@"files/" + ogFile);

        if(ogFileEXT == "csv") picker = 1;
        else if(ogFileEXT == "html") picker = 2;
        else if(ogFileEXT == "json") picker = 3;
        else if(ogFileEXT == "md") picker = 4;


        if(newGangsterFileEXT == "csv") otherPicker = 1;
        else if(newGangsterFileEXT == "html") otherPicker = 2;
        else if(newGangsterFileEXT == "json") otherPicker = 3;
        else if(newGangsterFileEXT == "md") otherPicker = 4;

        switch(picker){
            case 1:
            if(switcher) Console.WriteLine("Dissecting The File!...");
            tabcon.setCsv(raw);
            Console.WriteLine();
            break;
            case 2:
            if(switcher) Console.WriteLine("Dissecting The File!..");
            tabcon.setHtml(raw);
            break;
            case 3:
            if(switcher) Console.WriteLine("Dissecting The File!..");
            tabcon.setJson(raw);
            break;
            case 4:
            if(switcher) Console.WriteLine("Dissecting The File!..");
            tabcon.setMd(raw);
            break;
        }
        string path = "";
        switch(otherPicker){
            case 1:
            if(switcher){
                Console.WriteLine("Converting... to: " + newGangsterFile);
                Console.WriteLine(tabcon.getCsv());
            }
            path = @"files\xConvert." + "csv";
            File.WriteAllTextAsync(path, tabcon.getCsv());
            break;
            case 2:
            if(switcher){
                Console.WriteLine("Converting... to: " + newGangsterFile);
                Console.WriteLine(tabcon.getHtml());
            }
            path = @"files\xConvert." + "html";
            File.WriteAllTextAsync(path, tabcon.getHtml());
            break;
            case 3:
            if(switcher){
                Console.WriteLine("Converting... to: " + newGangsterFile);
                Console.WriteLine(tabcon.getJson());
            }
            path = @"files\xConvert." + "json";
            File.WriteAllTextAsync(path, tabcon.getJson());
            break;
            case 4:
            if(switcher){
                Console.WriteLine("Converting... to: " + newGangsterFile);
                Console.WriteLine(tabcon.getMd());
            }
            path = @"files\xConvert." + "md";
            File.WriteAllTextAsync(path, tabcon.getMd());
            break;
        }
    }
}

class table{
    private Boolean vb; // verbose
    public void verboseMode(){
        vb = true;
    }

    private string[] titles = {""};
    private List<string[]> info = new List<string[]>();


    public void setCsv(string s){
        string[] strArr = s.Split('\n');
        for(int i = 0; i < strArr.Length; i++){
            string[] strArrInd = strArr[i].Split(',');
            for(int u = 0; u < strArrInd.Length; u++){
                strArrInd[u] = strArrInd[u].Replace("\"", "");
                strArrInd[u] = Regex.Replace(strArrInd[u], @"\s+", "");
            }
            if(i == 0)  titles = strArrInd;
            else info.Add(strArrInd);
        }
    }

    public void setJson(string s){
        string[] strArr = s.Split('\n');
        List<string> strList = new List<string>();
        List<string> titList = new List<string>();
        int max = 0, j = 0;
        for(int u = 0; u < strArr.Length; u++){
            if(strArr[u].Contains("}")){
                j = 0;
            }

            if(!strArr[u].Contains("[") & !strArr[u].Contains("]") & !strArr[u].Contains("{") & !strArr[u].Contains("}")){
                string[] strArrInd = strArr[u].Split(":");

                for(int i = 0; i < strArrInd.Length; i++){
                    strArrInd[i] = Regex.Replace(strArrInd[i], @"\s+", "");
                    strArrInd[i] = strArrInd[i].Replace("\"", "");
                    strArrInd[i] = strArrInd[i].Replace(",", "");
                }
                titList.Add(strArrInd[0]);
                strList.Add(strArrInd[1]);
                j++;
                max = j;
            }
        }

        //add titles
        string[] temp = new string[max];

        for(int i = 0; i < max; i++){
            temp[i] = titList[i];
        }
        titles = temp.ToArray();
        
        //add info
        int l = 0;
        for(int i = 0; i < strList.Count; i++){
            temp[l] = strList[i];
            l++;
            if(l == max){
                info.Add(temp.ToArray());
                l = 0;
            }
        }
    }

    public void setHtml(string s){
        int max = 0, pq = 0, j = 0;
        
        string[] strArr = s.Split('\n');
        string[] strArrClean = new string[strArr.Length];

        for(int i = 0; i < strArr.Length; i++){
            if(strArr[i] != "<table>" || strArr[i] != "</table>" || strArr[i] != "<tr>" || strArr[i] != "</tr>"){
                strArr[i] = Regex.Replace(strArr[i], @"\s+", "");
                strArrClean[i] = strArr[i].Replace("<th>", "");
                strArrClean[i] = strArrClean[i].Replace("</th>", "");
                strArrClean[i] = strArrClean[i].Replace("</td>", "");
                strArrClean[i] = strArrClean[i].Replace("<td>", "");
                strArrClean[i] = strArrClean[i].Replace("<tdalign=\"\"right\"\">" , "");
            }
        }
        
        for(int i = 0; i < strArr.Length; i++){
            if(strArr[i].Contains("<th>")){
                j++;
                if(strArrClean[i] == "</tr>") break;
            }
        }
        string[] strTit = new string[j];

        for(int i = 0; i < strArr.Length; i++){
            if(strArr[i].Contains("<th>")){
                strTit[pq] = strArrClean[i];
                pq++;
            }
            if(strArr[i].Contains("</tr>")){
                titles = strTit;
                max = i + 1;
                break;
            }
        }

        string[] tempInfo = new string[titles.Length];

        pq = 0;
        for(int i = max; i < strArr.Length; i++){
            if(strArr[i].Contains("<td")){
                tempInfo[pq] = strArrClean[i];
                pq++;
            }
            if(strArr[i].Contains("</tr>")){
                info.Add(tempInfo.ToArray());
                pq = 0;
            }
        }
    }

    public void setMd(string s){
        string[] strArr = s.Split('\n');
        strArr[0] = Regex.Replace(strArr[0], @"\s+", "");
        string[] strArrNew = strArr[0].Split('|');
        string[] finTit = new string[strArrNew.Length - 2];
        int j = 0;
        for(int i = 1; i < strArrNew.Length - 1; i++){
            finTit[j] = strArrNew[i];
            j++;
        }
        titles = finTit;

        string[] finInfo = new string[j];
        for(int i = 2; i < strArr.Length; i++){
            strArr[i] = Regex.Replace(strArr[i], @"\s+", "");
            string[] temp = strArr[i].Split('|');
            for(int u = 1; u < j + 1; u++){
                finInfo[u - 1] = temp[u];
            }
            info.Add(finInfo.ToArray());
        } 
    }
    
    public string getJson(){

        string fin = "";

        fin = fin + "[" + "\n";
        for(int i = 0; i < info.Count; i++){
            fin = fin + "{\n";
            for(int j = 0; j < titles.Length; j++){
                if(j < titles.Length - 1) fin = fin + "\"" + titles[j] + "\": \"" + info[i][j] + "\",\n";
                else if(j < titles.Length) fin = fin + "\"" + titles[j] + "\": \"" + info[i][j] + "\"";
            }
            fin = fin + "\n}";
            if(i < info.Count - 1){
                fin = fin + ",";
            }
            fin = fin + "\n";
        }
        fin = fin + "]";
        return fin;
    }
    
    public string getCsv(){

        string fin = "";
        int cl = titles.Length;

        for(int i = 0; i < titles.Length; i++){
            if(i < titles.Length - 1) fin = fin + "\"" + titles[i].ToString() + "\"" + ",";
            else if(i == titles.Length - 1) fin = fin + "\"" + titles[i].ToString() + "\"";
        }

        
        for(int i = 0; i < titles.Length; i++){
            fin = fin + "\n";
            for(int c = 0; c < info.Count; c++){
                if(c < info[i].Length - 1) fin = fin + "\"" + info[i][c].ToString() + "\"" + ",";
                else if(c == info[i].Length - 1) fin = fin + "\"" + info[i][c].ToString() + "\"";
            }
        }

        return fin;
    }

    public string getHtml(){
        string fin = "";
        fin = fin + "<table>\n<tr>\n";

        for(int i = 0; i < titles.Length; i++){
            fin = fin + "<th>" + titles[i] + "</th>\n";
        }
        fin = fin + "</tr>\n";
        for(int u = 0; u < titles.Length; u++){
            fin = fin + "<tr>\n";
            for(int i = 0; i < info.Count; i++){
                fin = fin + "<td>" + info[u][i] + "</td>\n";
            }
            fin = fin + "</tr>\n";
        }

        fin = fin + "</table>";

        return fin;
    }

    public string getMd(){
        string fin = ""; 
        for(int i = 0; i < titles.Length; i++){
            fin = fin + "|" + titles[i];
        }
        fin = fin + "|\n";
        
        for(int i = 0; i < info.Count; i++){
            for(int u = 0; u < titles.Length; u++){
                fin = fin + "|" + info[i][u];
            }
            fin = fin + "|\n";
        }
        return fin;
    }
}
