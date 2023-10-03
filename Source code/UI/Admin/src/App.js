import "./App.css";
import Routers from "./Components/Routers/Routers";

function App() {
  console.log(process.env.REACT_APP_API_PATH);
  return (
<>
<Routers/>
</>
  );
}

export default App;
