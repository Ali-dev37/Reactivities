import axios from "axios";
import { useEffect, useState } from "react"
import { Header, Icon, List } from "semantic-ui-react";


function App() {
  const [activities,setActivities] = useState([]);

  useEffect(()=>{
    axios.get("http://localhost:5000/api/activities")
    .then((response)=>{
      console.log(response)
      setActivities(response.data)
    })
  },[])
  return (
    <>
    <Header as='h2'>
      <Icon name='settings' />
      <Header.Content>
        Reactivities
        {/* <Header.Subheader>Manage your preferences</Header.Subheader> */}
      </Header.Content>
    </Header>
    <List>
    {activities.map((activity:any)=>{
        return <List.Item key={activity.title}>{activity.title}</List.Item>
      })}
      
    </List>
    {/* <ul>
      {activities.map((activity:any)=>{
        return <span key={activity.title}>{activity.title}</span>
      })}
    </ul> */}
    </>
  )
}

export default App
