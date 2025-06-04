import { AuthProvider } from "./Pages/Context/AuthContext";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import  Home  from "./Pages/Home";
import Register from "./Pages/Regsiter";
import User from "./Pages/User/User";
import CreateGame from "./Pages/Games/CreateGame";
import ViewGames from "./Pages/Games/ViewGames";

function App() {

  return (
    <>
      <AuthProvider>
        <Router>
          <Routes>
            <Route path ="/" element={<Home />} />
            <Route path ="/register" element={<Register />} />
            <Route path ="/profile/:id" element={<User />}/>
            <Route path="/games/create" element={<CreateGame />}/>
            <Route path="/games" element={<ViewGames />}/>
            <Route path="/games/:id" element={<ViewGames />}/>
          </Routes>
        </Router>
      </AuthProvider>
    </>
  )
}

export default App
