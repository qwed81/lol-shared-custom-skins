import React from 'react';
import '../css/components/App.scss';
import { Menu } from './Menu';
import { Main } from './Main';
import { Party } from './Party/Party';

const App = () => {
  return (
    <div className='app-component'>
      <Menu/>
      <Party/>
      <Main/>
    </div>
  );
}

export default App;
