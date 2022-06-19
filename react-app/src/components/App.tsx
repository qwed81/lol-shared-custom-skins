import React from 'react';
import '../css/components/App.scss';
import { Menu } from './Menu';
import { Main } from './Main/Main';
import { Party } from './Party/PartyMain';
import { FilterContextProvider } from './FilterContext';

const App = () => {
  return (
    <div className='app-component'>
      <FilterContextProvider>
        <Menu/>
        <Party/>
        <Main/>
      </FilterContextProvider>
    </div>
  );
}

export default App;
