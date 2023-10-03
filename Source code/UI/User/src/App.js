import "./App.css";
import RouterFile from "./Routes/RouterFile";
function App() {
  console.log(process.env.REACT_APP_API_PATH);
  return (
    <>
      <RouterFile />
    </>
  );
}

export default App;
